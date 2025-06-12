using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/expense-categories/sync")]
    public class ExpenseCategoryControllerSync : ControllerBase
    {
        private readonly IExpenseCategoryAppService _service;
        private readonly ITrafficAppService _serviceTraffic;
        private readonly ILogger<ExpenseCategoryControllerSync> _logger;

        public ExpenseCategoryControllerSync(IExpenseCategoryAppService service, ITrafficAppService serviceTraffic, ILogger<ExpenseCategoryControllerSync> logger)
        {
            _service = service;
            _serviceTraffic = serviceTraffic;
            _logger = logger;
        }

        #region CRUD Operations

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                LogTraffic("GET - GetAll - ExpenseCategory - Sync", "Request");

                var result = _service.GetAllSync();

                LogTraffic("GET - GetAll - ExpenseCategory - Sync", "Response");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting expenses categories");
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("{id:long}")]
        public IActionResult GetById(long id)
        {
            try
            {
                LogTraffic("GET - GetById - ExpenseCategory - Sync", "Request");

                var result = _service.GetByIdSync(id);

                LogTraffic("GET - GetById - ExpenseCategory - Sync", "Response");

                if (result is null)
                {
                    _logger.LogWarning("Expense with id {ExpenseCategoryId} not found", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving expense category with id {ExpenseCategoryId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] ExpenseCategoryDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for expense creation: {@ExpenseDto}", dto);
                    return BadRequest(ModelState);
                }

                LogTraffic("POST - Create - ExpenseCategory - Sync", "Request");

                _service.AddSync(dto);

                LogTraffic("POST - Create - ExpenseCategory - Sync", "Response");

                _logger.LogInformation("Expense category created with id {ExpenseCategoryId}", dto.Id);

                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating expense category: {@ExpenseCategoryDto}", dto);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPut("{id:long}")]
        public IActionResult Update(long id, [FromBody] ExpenseCategoryDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for expense category update: {@ExpenseCategoryDto}", dto);
                    return BadRequest(ModelState);
                }

                if (id != dto.Id)
                {
                    _logger.LogWarning("Expense ID mismatch: route id = {RouteId}, dto id = {DtoId}", id, dto.Id);
                    return BadRequest("ID mismatch");
                }

                LogTraffic("PUT - Update - ExpenseCategory - Sync", "Request");

                var existingExpenseCategory = _service.GetByIdSync(id);
                if (existingExpenseCategory is null)
                {
                    _logger.LogWarning("Expense category with id {ExpenseCategoryId} not found for update", id);
                    return NotFound();
                }

                _service.UpdateSync(dto);

                LogTraffic("PUT - Update - ExpenseCategory - Sync", "Response");

                _logger.LogInformation("Expense category with id {ExpenseCategoryId} updated successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating expense category with id {ExpenseCategoryId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            try
            {
                LogTraffic("DELETE - Delete - ExpenseCategory - Sync", "Request");

                var existingExpenseCategory = _service.GetByIdSync(id);
                if (existingExpenseCategory is null)
                {
                    _logger.LogWarning("Expense category with id {ExpenseCategoryId} not found for deletion", id);
                    return NotFound();
                }

                _service.DeleteSync(existingExpenseCategory);

                LogTraffic("DELETE - Delete - ExpenseCategory - Sync", "Response");

                _logger.LogInformation("Expense category with id {ExpenseCategoryId} deleted successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting expense category with id {ExpenseCategoryId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("filtered")]
        public IActionResult GetAllFiltered([FromQuery] string filterBy, [FromQuery] string value)
        {
            try
            {
                LogTraffic("GET - GetAllFiltered - ExpenseCategory - Sync", "Request");

                var result = _service.GetAllFilteredSync(filterBy, value);

                LogTraffic("GET - GetAllFiltered - ExpenseCategory - Sync", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering expense category by {FilterBy} ", filterBy);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("ordered")]
        public IActionResult GetAllOrdered([FromQuery] string orderBy, [FromQuery] string direction = "asc")
        {
            try
            {
                LogTraffic("GET - GetAllOrdered - ExpenseCategory - Sync", "Request");

                var result = _service.GetAllOrderedSync(orderBy, direction);

                LogTraffic("GET - GetAllOrdered - ExpenseCategory - Sync", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ordering expenses categories by {OrderBy} {Direction}", orderBy, direction);
                return StatusCode(500, "Error processing request");
            }
        }

        #endregion

        #region Ordered Methods Additional

        #endregion

        #region Additional Methods Private 

        private void LogTraffic(string method, string action)
        {
            var dto = new TrafficDto(method, action, DateTime.Now);
            _serviceTraffic.AddSync(dto);
        }

        #endregion
    }
}
