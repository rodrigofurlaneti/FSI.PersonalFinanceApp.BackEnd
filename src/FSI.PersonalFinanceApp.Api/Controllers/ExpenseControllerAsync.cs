using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/expenses/async")]
    public class ExpenseControllerAsync : ControllerBase
    {
        private readonly IExpenseAppService _service;
        private readonly ITrafficAppService _serviceTraffic;
        private readonly ILogger<ExpenseControllerAsync> _logger;

        public ExpenseControllerAsync(IExpenseAppService service, ITrafficAppService serviceTraffic, ILogger<ExpenseControllerAsync> logger)
        {
            _service = service;
            _serviceTraffic = serviceTraffic;
            _logger = logger;
        }

        #region CRUD Operations

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                await LogTraffic("GET - GetAll - Expense - Async", "Request");

                var result = await _service.GetAllAsync();

                await LogTraffic("GET - GetAll - Expense - Async", "Response");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting expenses");
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                await LogTraffic("GET - GetById - Expense - Async", "Request");

                var result = await _service.GetByIdAsync(id);

                await LogTraffic("GET - GetById - Expense - Async", "Response");

                if (result is null)
                {
                    _logger.LogWarning("Expense with id {ExpenseId} not found", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving expense with id {ExpenseId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExpenseDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for expense creation: {@ExpenseDto}", dto);
                    return BadRequest(ModelState);
                }

                await LogTraffic("POST - Create - Expense - Async", "Request");

                await _service.AddAsync(dto);

                await LogTraffic("POST - Create - Expense - Async", "Response");

                _logger.LogInformation("Expense created with id {ExpenseId}", dto.Id);

                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating expense: {@ExpenseDto}", dto);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] ExpenseDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for expense update: {@ExpenseDto}", dto);
                    return BadRequest(ModelState);
                }

                if (id != dto.Id)
                {
                    _logger.LogWarning("Expense ID mismatch: route id = {RouteId}, dto id = {DtoId}", id, dto.Id);
                    return BadRequest("ID mismatch");
                }

                await LogTraffic("PUT - Update - Expense - Async", "Request");

                var existingExpense = await _service.GetByIdAsync(id);
                if (existingExpense is null)
                {
                    _logger.LogWarning("Expense with id {ExpenseId} not found for update", id);
                    return NotFound();
                }

                await _service.UpdateAsync(dto);

                await LogTraffic("PUT - Update - Expense - Async", "Response");

                _logger.LogInformation("Expense with id {ExpenseId} updated successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating expense with id {ExpenseId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await LogTraffic("DELETE - Delete - Expense - Async", "Request");

                var existingExpense = await _service.GetByIdAsync(id);
                if (existingExpense is null)
                {
                    _logger.LogWarning("Expense with id {ExpenseId} not found for deletion", id);
                    return NotFound();
                }

                await _service.DeleteAsync(existingExpense);

                await LogTraffic("DELETE - Delete - Expense - Async", "Response");

                _logger.LogInformation("Expense with id {ExpenseId} deleted successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting expense with id {ExpenseId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetAllFiltered([FromQuery] string filterBy, [FromQuery] string value)
        {
            try
            {
                await LogTraffic("GET - GetAllFiltered - Expense - Async", "Request");

                var result = await _service.GetAllFilteredAsync(filterBy, value);

                await LogTraffic("GET - GetAllFiltered - Expense - Async", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering expenses by {FilterBy} ", filterBy);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("ordered")]
        public async Task<IActionResult> GetAllOrdered([FromQuery] string orderBy, [FromQuery] string direction = "asc")
        {
            try
            {
                await LogTraffic("GET - GetAllOrdered - Expense - Async", "Request");

                var result = await _service.GetAllOrderedAsync(orderBy, direction);

                await LogTraffic("GET - GetAllOrdered - Expense - Async", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ordering expenses by {OrderBy} {Direction}", orderBy, direction);
                return StatusCode(500, "Error processing request");
            }
        }



        #endregion

        #region Additional Methods  

        #endregion

        #region Additional Methods Private 

        private async Task LogTraffic(string method, string action)
        {
            var dto = new TrafficDto(method, action, DateTime.Now);
            await _serviceTraffic.AddAsync(dto);
        }

        #endregion
    }
}
