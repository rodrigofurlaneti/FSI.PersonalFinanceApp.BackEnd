using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/income-categories/sync")]
    public class IncomeCategoryControllerSync : ControllerBase
    {
        private readonly IIncomeCategoryAppService _service;
        private readonly ITrafficAppService _serviceTraffic;
        private readonly ILogger<IncomeCategoryControllerSync> _logger;

        public IncomeCategoryControllerSync(IIncomeCategoryAppService service, ITrafficAppService serviceTraffic, ILogger<IncomeCategoryControllerSync> logger)
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
                LogTraffic("GET - GetAll - IncomeCategory - Sync", "Request");

                var result = _service.GetAllSync();

                LogTraffic("GET - GetAll - IncomeCategory - Sync", "Response");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting financial goal");
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("{id:long}")]
        public IActionResult GetById(long id)
        {
            try
            {
                LogTraffic("GET - GetById - IncomeCategory - Sync", "Request");

                var result = _service.GetByIdSync(id);

                LogTraffic("GET - GetById - IncomeCategory - Sync", "Response");

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
        public IActionResult Create([FromBody] IncomeCategoryDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for income category creation: {@IncomeCategoryDto}", dto);
                    return BadRequest(ModelState);
                }

                LogTraffic("POST - Create - IncomeCategory - Sync", "Request");

                _service.AddSync(dto);

                LogTraffic("POST - Create - IncomeCategory - Sync", "Response");

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
        public IActionResult Update(long id, [FromBody] IncomeCategoryDto dto)
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

                LogTraffic("PUT - Update - IncomeCategory - Sync", "Request");

                var existingIncomeCategory = _service.GetByIdSync(id);
                if (existingIncomeCategory is null)
                {
                    _logger.LogWarning("Income category with id {IncomeCategoryId} not found for update", id);
                    return NotFound();
                }

                _service.UpdateSync(dto);

                LogTraffic("PUT - Update - IncomeCategory - Sync", "Response");

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
        public IActionResult Delete(long id)
        {
            try
            {
                LogTraffic("DELETE - Delete - IncomeCategory - Sync", "Request");

                var existingIncomeCategory = _service.GetByIdSync(id);
                if (existingIncomeCategory is null)
                {
                    _logger.LogWarning("Income category with id {IncomeCategoryId} not found for deletion", id);
                    return NotFound();
                }

                _service.DeleteSync(existingIncomeCategory);

                LogTraffic("DELETE - Delete - IncomeCategory - Sync", "Response");

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
        public IActionResult GetAllFiltered([FromQuery] string filterBy, [FromQuery] string value)
        {
            try
            {
                LogTraffic("GET - GetAllFiltered - IncomeCategory - Sync", "Request");

                var result = _service.GetAllFilteredSync(filterBy, value);

                LogTraffic("GET - GetAllFiltered - IncomeCategory - Sync", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering income category by {FilterBy} ", filterBy);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("ordered")]
        public IActionResult GetAllOrdered([FromQuery] string orderBy, [FromQuery] string direction = "asc")
        {
            try
            {
                LogTraffic("GET - GetAllOrdered - IncomeCategory - Sync", "Request");

                var result = _service.GetAllOrderedSync(orderBy, direction);

                LogTraffic("GET - GetAllOrdered - IncomeCategory - Sync", "Response");

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

        #endregion

        #region Additional Methods Private 

        private async Task LogTraffic(string method, string action)
        {
            var dto = new TrafficDto(method, action, DateTime.Now);
            _serviceTraffic.AddSync(dto);
        }

        #endregion
    }
}
