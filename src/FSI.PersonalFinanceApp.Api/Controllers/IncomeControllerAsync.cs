using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/incomes/async")]
    public class IncomeControllerAsync : ControllerBase
    {
        private readonly IIncomeAppService _service;
        private readonly ITrafficAppService _serviceTraffic;
        private readonly ILogger<IncomeControllerAsync> _logger;

        public IncomeControllerAsync(IIncomeAppService service, ITrafficAppService serviceTraffic, ILogger<IncomeControllerAsync> logger)
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
                await LogTraffic("GET - GetAll - Income - Async", "Request");

                var result = await _service.GetAllAsync();

                await LogTraffic("GET - GetAll - Income - Async", "Response");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting income");
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                await LogTraffic("GET - GetById - Income - Async", "Request");

                var result = await _service.GetByIdAsync(id);

                await LogTraffic("GET - GetById - Income - Async", "Response");

                if (result is null)
                {
                    _logger.LogWarning("Income with id {IncomeId} not found", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving income with id {IncomeId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IncomeDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for income creation: {@IncomeDto}", dto);
                    return BadRequest(ModelState);
                }

                await LogTraffic("POST - Create - Income - Async", "Request");

                await _service.AddAsync(dto);

                await LogTraffic("POST - Create - Income - Async", "Response");

                _logger.LogInformation("Income created with id {IncomeId}", dto.Id);

                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating income: {@IncomeDto}", dto);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] IncomeDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for income update: {@IncomeDto}", dto);
                    return BadRequest(ModelState);
                }

                if (id != dto.Id)
                {
                    _logger.LogWarning("Income ID mismatch: route id = {RouteId}, dto id = {DtoId}", id, dto.Id);
                    return BadRequest("ID mismatch");
                }

                await LogTraffic("PUT - Update - Income - Async", "Request");

                var existingIncome = await _service.GetByIdAsync(id);
                if (existingIncome is null)
                {
                    _logger.LogWarning("Income with id {IncomeId} not found for update", id);
                    return NotFound();
                }

                await _service.UpdateAsync(dto);

                await LogTraffic("PUT - Update - Income - Async", "Response");

                _logger.LogInformation("Income with id {IncomeId} updated successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating income with id {IncomeId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await LogTraffic("DELETE - Delete - Income - Async", "Request");

                var existingIncome = await _service.GetByIdAsync(id);
                if (existingIncome is null)
                {
                    _logger.LogWarning("Income with id {IncomeId} not found for deletion", id);
                    return NotFound();
                }

                await _service.DeleteAsync(existingIncome);

                await LogTraffic("DELETE - Delete - Income - Async", "Response");

                _logger.LogInformation("Income with id {IncomeId} deleted successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting income with id {IncomeId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetAllFiltered([FromQuery] string filterBy, [FromQuery] string value)
        {
            try
            {
                await LogTraffic("GET - GetAllFiltered - Income - Async", "Request");

                var result = await _service.GetAllFilteredAsync(filterBy, value);

                await LogTraffic("GET - GetAllFiltered - Income - Async", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering income by {FilterBy} ", filterBy);
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
