using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/traffics/async")]
    public class TrafficControllerAsync : ControllerBase
    {
        private readonly ITrafficAppService _service;
        private readonly ILogger<TrafficControllerAsync> _logger;

        public TrafficControllerAsync(ITrafficAppService service, ILogger<TrafficControllerAsync> logger)
        {
            _service = service;
            _logger = logger;
        }

        #region CRUD Operations

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                await LogTraffic("GET - GetAll - Traffic - Async", "Request");

                var result = await _service.GetAllAsync();

                await LogTraffic("GET - GetAll - Traffic - Async", "Response");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting traffic");
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                await LogTraffic("GET - GetById - Traffic - Async", "Request");

                var result = await _service.GetByIdAsync(id);

                await LogTraffic("GET - GetById - Traffic - Async", "Response");

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
        public async Task<IActionResult> Create([FromBody] TrafficDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for traffic creation: {@TrafficDto}", dto);
                    return BadRequest(ModelState);
                }

                await LogTraffic("POST - Create - Traffic - Async", "Request");

                await _service.AddAsync(dto);

                await LogTraffic("POST - Create - Traffic - Async", "Response");

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
        public async Task<IActionResult> Update(long id, [FromBody] TrafficDto dto)
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

                await LogTraffic("PUT - Update - Traffic - Async", "Request");

                var existingTraffic = await _service.GetByIdAsync(id);
                if (existingTraffic is null)
                {
                    _logger.LogWarning("Traffic with id {TrafficId} not found for update", id);
                    return NotFound();
                }

                await _service.UpdateAsync(dto);

                await LogTraffic("PUT - Update - Traffic - Async", "Response");

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
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await LogTraffic("DELETE - Delete - Traffic - Async", "Request");

                var existingTraffic = await _service.GetByIdAsync(id);
                if (existingTraffic is null)
                {
                    _logger.LogWarning("Traffic with id {TrafficId} not found for deletion", id);
                    return NotFound();
                }

                await _service.DeleteAsync(existingTraffic);

                await LogTraffic("DELETE - Delete - Traffic - Async", "Response");

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
        public async Task<IActionResult> GetAllFiltered([FromQuery] string filterBy, [FromQuery] string value)
        {
            try
            {
                await LogTraffic("GET - GetAllFiltered - Traffic - Async", "Request");

                var result = await _service.GetAllFilteredAsync(filterBy, value);

                await LogTraffic("GET - GetAllFiltered - Traffic - Async", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering traffic by {FilterBy} ", filterBy);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("ordered")]
        public async Task<IActionResult> GetAllOrdered([FromQuery] string orderBy, [FromQuery] string direction = "asc")
        {
            try
            {
                await LogTraffic("GET - GetAllOrdered - Traffic - Async", "Request");

                var result = await _service.GetAllOrderedAsync(orderBy, direction);

                await LogTraffic("GET - GetAllOrdered - Traffic - Async", "Response");

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
            await _service.AddAsync(dto);
        }

        #endregion
    }
}
