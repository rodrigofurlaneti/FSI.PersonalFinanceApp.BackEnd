using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/incomes/sync")]
    public class IncomeControllerSync : ControllerBase
    {
        private readonly IIncomeAppService _service;
        private readonly ITrafficAppService _serviceTraffic;
        private readonly ILogger<IncomeControllerSync> _logger;

        public IncomeControllerSync(IIncomeAppService service, ITrafficAppService serviceTraffic, ILogger<IncomeControllerSync> logger)
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
                LogTraffic("GET - GetAll - Income - Sync", "Request");

                var result = _service.GetAllSync();

                LogTraffic("GET - GetAll - Income - Sync", "Response");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting income");
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("{id:long}")]
        public IActionResult GetById(long id)
        {
            try
            {
                LogTraffic("GET - GetById - Income - Sync", "Request");

                var result = _service.GetByIdSync(id);

                LogTraffic("GET - GetById - Income - Sync", "Response");

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
        public IActionResult Create([FromBody] IncomeDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for income creation: {@IncomeDto}", dto);
                    return BadRequest(ModelState);
                }

                LogTraffic("POST - Create - Income - Sync", "Request");

                _service.AddSync(dto);

                LogTraffic("POST - Create - Income - Sync", "Response");

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
        public IActionResult Update(long id, [FromBody] IncomeDto dto)
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

                LogTraffic("PUT - Update - Income - Sync", "Request");

                var existingIncome = _service.GetByIdSync(id);
                if (existingIncome is null)
                {
                    _logger.LogWarning("Income with id {IncomeId} not found for update", id);
                    return NotFound();
                }

                _service.UpdateSync(dto);

                LogTraffic("PUT - Update - Income - Sync", "Response");

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
        public IActionResult Delete(long id)
        {
            try
            {
                LogTraffic("DELETE - Delete - Income - Sync", "Request");

                var existingIncome = _service.GetByIdSync(id);
                if (existingIncome is null)
                {
                    _logger.LogWarning("Income with id {IncomeId} not found for deletion", id);
                    return NotFound();
                }

                _service.DeleteSync(existingIncome);

                LogTraffic("DELETE - Delete - Income - Sync", "Response");

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
        public IActionResult GetAllFiltered([FromQuery] string filterBy, [FromQuery] string value)
        {
            try
            {
                LogTraffic("GET - GetAllFiltered - Income - Sync", "Request");

                var result = _service.GetAllFilteredSync(filterBy, value);

                LogTraffic("GET - GetAllFiltered - Income - Sync", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering income by {FilterBy} ", filterBy);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("ordered")]
        public IActionResult GetAllOrdered([FromQuery] string orderBy, [FromQuery] string direction = "asc")
        {
            try
            {
                LogTraffic("GET - GetAllOrdered - Income - Sync", "Request");

                var result = _service.GetAllOrderedSync(orderBy, direction);

                LogTraffic("GET - GetAllOrdered - Income - Sync", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ordering income by {OrderBy} {Direction}", orderBy, direction);
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
