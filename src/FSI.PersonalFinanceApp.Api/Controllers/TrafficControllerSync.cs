using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/traffics/sync")]
    public class TrafficControllerSync : ControllerBase
    {
        private readonly ITrafficAppService _service;
        private readonly ILogger<TrafficControllerSync> _logger;

        public TrafficControllerSync(ITrafficAppService service, ILogger<TrafficControllerSync> logger)
        {
            _service = service;
            _logger = logger;
        }

        #region CRUD Operations

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                LogTraffic("GET - GetAll - Traffic - Sync", "Request");

                var result = _service.GetAllSync();

                LogTraffic("GET - GetAll - Traffic - Sync", "Response");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting traffic");
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("{id:long}")]
        public IActionResult GetById(long id)
        {
            try
            {
                LogTraffic("GET - GetById - Traffic - Sync", "Request");

                var result = _service.GetByIdSync(id);

                LogTraffic("GET - GetById - Traffic - Sync", "Response");

                if (result is null)
                {
                    _logger.LogWarning("Traffic with id {TrafficId} not found", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving traffic with id {TrafficId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] TrafficDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for traffic creation: {@TrafficDto}", dto);
                    return BadRequest(ModelState);
                }

                LogTraffic("POST - Create - Traffic - Sync", "Request");

                _service.AddSync(dto);

                LogTraffic("POST - Create - Traffic - Sync", "Response");

                _logger.LogInformation("Traffic created with id {TrafficId}", dto.Id);

                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating traffic: {@TrafficDto}", dto);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPut("{id:long}")]
        public IActionResult Update(long id, [FromBody] TrafficDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for traffic update: {@TrafficDto}", dto);
                    return BadRequest(ModelState);
                }

                if (id != dto.Id)
                {
                    _logger.LogWarning("Traffic ID mismatch: route id = {RouteId}, dto id = {DtoId}", id, dto.Id);
                    return BadRequest("ID mismatch");
                }

                LogTraffic("PUT - Update - Traffic - Sync", "Request");

                var existingTraffic = _service.GetByIdSync(id);
                if (existingTraffic is null)
                {
                    _logger.LogWarning("Traffic with id {TrafficId} not found for update", id);
                    return NotFound();
                }

                _service.UpdateSync(dto);

                LogTraffic("PUT - Update - Traffic - Sync", "Response");

                _logger.LogInformation("Traffic with id {TrafficId} updated successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating traffic with id {TrafficId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            try
            {
                LogTraffic("DELETE - Delete - Traffic - Sync", "Request");

                var existingTraffic = _service.GetByIdSync(id);
                if (existingTraffic is null)
                {
                    _logger.LogWarning("Traffic with id {TrafficId} not found for deletion", id);
                    return NotFound();
                }

                _service.DeleteSync(existingTraffic);

                LogTraffic("DELETE - Delete - Traffic - Sync", "Response");

                _logger.LogInformation("Traffic with id {TrafficId} deleted successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting traffic with id {TrafficId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("filtered")]
        public IActionResult GetAllFiltered([FromQuery] string filterBy, [FromQuery] string value)
        {
            try
            {
                LogTraffic("GET - GetAllFiltered - Traffic - Sync", "Request");

                var result = _service.GetAllFilteredSync(filterBy, value);

                LogTraffic("GET - GetAllFiltered - Traffic - Sync", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering traffic by {FilterBy} ", filterBy);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("ordered")]
        public IActionResult GetAllOrdered([FromQuery] string orderBy, [FromQuery] string direction = "asc")
        {
            try
            {
                LogTraffic("GET - GetAllOrdered - Traffic - Sync", "Request");

                var result = _service.GetAllOrderedSync(orderBy, direction);

                LogTraffic("GET - GetAllOrdered - Traffic - Sync", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ordering traffic by {OrderBy} {Direction}", orderBy, direction);
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
            _service.AddSync(dto);
        }

        #endregion
    }
}
