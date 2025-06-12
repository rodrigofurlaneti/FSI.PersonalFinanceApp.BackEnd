using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/income-categories/async")]
    public class IncomeCategoryControllerAsync : ControllerBase
    {
        private readonly IIncomeCategoryAppService _service;
        private readonly ITrafficAppService _serviceTraffic;
        private readonly ILogger<IncomeCategoryControllerAsync> _logger;

        public IncomeCategoryControllerAsync(IIncomeCategoryAppService service, ITrafficAppService serviceTraffic, ILogger<IncomeCategoryControllerAsync> logger)
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
                await LogTraffic("GET - GetAll - IncomeCategory - Async", "Request");

                var result = await _service.GetAllAsync();

                await LogTraffic("GET - GetAll - IncomeCategory - Async", "Response");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting income category");
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                await LogTraffic("GET - GetById - IncomeCategory - Async", "Request");

                var result = await _service.GetByIdAsync(id);

                await LogTraffic("GET - GetById - IncomeCategory - Async", "Response");

                if (result is null)
                {
                    _logger.LogWarning("Income category with id {IncomeCategoryId} not found", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving income category with id {IncomeCategoryId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IncomeCategoryDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for income category creation: {@IncomeCategoryDto}", dto);
                    return BadRequest(ModelState);
                }

                await LogTraffic("POST - Create - IncomeCategory - Async", "Request");

                await _service.AddAsync(dto);

                await LogTraffic("POST - Create - IncomeCategory - Async", "Response");

                _logger.LogInformation("Income category created with id {IncomeCategoryId}", dto.Id);

                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating income category: {@IncomeCategoryDto}", dto);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] IncomeCategoryDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for income category update: {@IncomeCategoryDto}", dto);
                    return BadRequest(ModelState);
                }

                if (id != dto.Id)
                {
                    _logger.LogWarning("Income category ID mismatch: route id = {RouteId}, dto id = {DtoId}", id, dto.Id);
                    return BadRequest("ID mismatch");
                }

                await LogTraffic("PUT - Update - IncomeCategory - Async", "Request");

                var existingIncomeCategory = await _service.GetByIdAsync(id);
                if (existingIncomeCategory is null)
                {
                    _logger.LogWarning("Income category with id {IncomeCategoryId} not found for update", id);
                    return NotFound();
                }

                await _service.UpdateAsync(dto);

                await LogTraffic("PUT - Update - IncomeCategory - Async", "Response");

                _logger.LogInformation("Income category with id {IncomeCategoryId} updated successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating income category with id {IncomeCategoryId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await LogTraffic("DELETE - Delete - IncomeCategory - Async", "Request");

                var existingIncomeCategory = await _service.GetByIdAsync(id);
                if (existingIncomeCategory is null)
                {
                    _logger.LogWarning("Income category with id {IncomeCategoryId} not found for deletion", id);
                    return NotFound();
                }

                await _service.DeleteAsync(existingIncomeCategory);

                await LogTraffic("DELETE - Delete - IncomeCategory - Async", "Response");

                _logger.LogInformation("Income category with id {IncomeCategoryId} deleted successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting income category with id {IncomeCategoryId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetAllFiltered([FromQuery] string filterBy, [FromQuery] string value)
        {
            try
            {
                await LogTraffic("GET - GetAllFiltered - IncomeCategory - Async", "Request");

                var result = await _service.GetAllFilteredAsync(filterBy, value);

                await LogTraffic("GET - GetAllFiltered - IncomeCategory - Async", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering income category by {FilterBy} ", filterBy);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("ordered")]
        public async Task<IActionResult> GetAllOrdered([FromQuery] string orderBy, [FromQuery] string direction = "asc")
        {
            try
            {
                await LogTraffic("GET - GetAllOrdered - IncomeCategory - Async", "Request");

                var result = await _service.GetAllOrderedAsync(orderBy, direction);

                await LogTraffic("GET - GetAllOrdered - IncomeCategory - Async", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ordering income category by {OrderBy} {Direction}", orderBy, direction);
                return StatusCode(500, "Error processing request");
            }
        }

        #endregion

        #region Additional Methods
        // Add any additional methods specific to income categories here, if needed.
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
