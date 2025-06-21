using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/financial-goals/sync")]
    public class FinancialGoalControllerSync : ControllerBase
    {
        private readonly IFinancialGoalAppService _service;
        private readonly ITrafficAppService _serviceTraffic;
        private readonly ILogger<FinancialGoalControllerSync> _logger;

        public FinancialGoalControllerSync(IFinancialGoalAppService service, ITrafficAppService serviceTraffic, ILogger<FinancialGoalControllerSync> logger)
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
                LogTraffic("GET - GetAll - FinancialGoal - Sync", "Request");

                var result = _service.GetAllSync();

                LogTraffic("GET - GetAll - FinancialGoal - Sync", "Response");

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
                LogTraffic("GET - GetById - FinancialGoal - Sync", "Request");

                var result = _service.GetByIdSync(id);

                LogTraffic("GET - GetById - FinancialGoal - Sync", "Response");

                if (result is null)
                {
                    _logger.LogWarning("FinancialGoal with id {FinancialGoalId} not found", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving financial goal with id {FinancialGoalId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] FinancialGoalDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for financial goal creation: {@FinancialGoalDto}", dto);
                    return BadRequest(ModelState);
                }

                LogTraffic("POST - Create - FinancialGoal - Sync", "Request");

                _service.AddSync(dto);

                LogTraffic("POST - Create - FinancialGoal - Sync", "Response");

                _logger.LogInformation("Financial goal created with id {FinancialGoalId}", dto.Id);

                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating financial goal: {@FinancialGoalDto}", dto);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPut("{id:long}")]
        public IActionResult Update(long id, [FromBody] FinancialGoalDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for financial goal update: {@FinancialGoalDto}", dto);
                    return BadRequest(ModelState);
                }

                if (id != dto.Id)
                {
                    _logger.LogWarning("Financial goal ID mismatch: route id = {RouteId}, dto id = {DtoId}", id, dto.Id);
                    return BadRequest("ID mismatch");
                }

                LogTraffic("PUT - Update - FinancialGoal - Sync", "Request");

                var existingFinancialGoal = _service.GetByIdSync(id);
                if (existingFinancialGoal is null)
                {
                    _logger.LogWarning("FinancialGoal with id {FinancialGoalId} not found for update", id);
                    return NotFound();
                }

                _service.UpdateSync(dto);

                LogTraffic("PUT - Update - FinancialGoal - Sync", "Response");

                _logger.LogInformation("FinancialGoal with id {FinancialGoalId} updated successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating income with id {FinancialGoalId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            try
            {
                LogTraffic("DELETE - Delete - FinancialGoal - Sync", "Request");

                var existingFinancialGoal = _service.GetByIdSync(id);
                if (existingFinancialGoal is null)
                {
                    _logger.LogWarning("Financial goal with id {FinancialGoalId} not found for deletion", id);
                    return NotFound();
                }

                _service.DeleteSync(existingFinancialGoal);

                LogTraffic("DELETE - Delete - FinancialGoal - Sync", "Response");

                _logger.LogInformation("Financial goal with id {FinancialGoalId} deleted successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting financial goal with id {FinancialGoalId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("filtered")]
        public IActionResult GetAllFiltered([FromQuery] string filterBy, [FromQuery] string value)
        {
            try
            {
                LogTraffic("GET - GetAllFiltered - FinancialGoal - Sync", "Request");

                var result = _service.GetAllFilteredSync(filterBy, value);

                LogTraffic("GET - GetAllFiltered - FinancialGoal - Sync", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering financial goal by {FilterBy} ", filterBy);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("ordered")]
        public IActionResult GetAllOrdered([FromQuery] string orderBy, [FromQuery] string direction = "asc")
        {
            try
            {
                LogTraffic("GET - GetAllOrdered - FinancialGoal - Sync", "Request");

                var result = _service.GetAllOrderedSync(orderBy, direction);

                LogTraffic("GET - GetAllOrdered - FinancialGoal - Sync", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ordering financial goal by {OrderBy} {Direction}", orderBy, direction);
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
