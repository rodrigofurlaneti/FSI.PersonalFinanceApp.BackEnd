using FSI.PersonalFinanceApp.Api.Controllers.Base;
using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Application.Messaging;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/traffics/async")]
    public class TrafficControllerAsync : BaseAsyncController<TrafficDto>
    {
        private readonly ITrafficAppService _service;

        public TrafficControllerAsync(ITrafficAppService service, ITrafficAppService trafficService, ILogger<TrafficControllerAsync> logger,
            IMessageQueuePublisher publisher, IMessagingAppService messagingService) : base(logger, publisher, messagingService, trafficService)
        {
            _service = service;
        }

        #region CRUD Operations

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                await LogTrafficAsync("GET - GetAll - Traffic - Async", "Request");

                var result = await _service.GetAllAsync();

                await LogTrafficAsync("GET - GetAll - Traffic - Async", "Response");

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
                await LogTrafficAsync("GET - GetById - Traffic - Async", "Request");

                var result = await _service.GetByIdAsync(id);

                await LogTrafficAsync("GET - GetById - Traffic - Async", "Response");

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

                await LogTrafficAsync("POST - Create - Traffic - Async", "Request");

                await _service.AddAsync(dto);

                await LogTrafficAsync("POST - Create - Traffic - Async", "Response");

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

                await LogTrafficAsync("PUT - Update - Traffic - Async", "Request");

                var existingTraffic = await _service.GetByIdAsync(id);
                if (existingTraffic is null)
                {
                    _logger.LogWarning("Traffic with id {TrafficId} not found for update", id);
                    return NotFound();
                }

                await _service.UpdateAsync(dto);

                await LogTrafficAsync("PUT - Update - Traffic - Async", "Response");

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
                await LogTrafficAsync("DELETE - Delete - Traffic - Async", "Request");

                var existingTraffic = await _service.GetByIdAsync(id);
                if (existingTraffic is null)
                {
                    _logger.LogWarning("Traffic with id {TrafficId} not found for deletion", id);
                    return NotFound();
                }

                await _service.DeleteAsync(existingTraffic);

                await LogTrafficAsync("DELETE - Delete - Traffic - Async", "Response");

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
                await LogTrafficAsync("GET - GetAllFiltered - Traffic - Async", "Request");

                var result = await _service.GetAllFilteredAsync(filterBy, value);

                await LogTrafficAsync("GET - GetAllFiltered - Traffic - Async", "Response");

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
                await LogTrafficAsync("GET - GetAllOrdered - Traffic - Async", "Request");

                var result = await _service.GetAllOrderedAsync(orderBy, direction);

                await LogTrafficAsync("GET - GetAllOrdered - Traffic - Async", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ordering traffic by {OrderBy} {Direction}", orderBy, direction);
                return StatusCode(500, "Error processing request");
            }
        }

        #endregion

        #region CRUD Operations Async - Event Driven Architecture - Request response via polling - Async Message Dispatch with Deferred Response

        [HttpPost("event/getall")]
        public async Task<IActionResult> MessageGetAllAsync()
        {
            await LogTrafficAsync("POST - MessageGetAllAsync", "Request");
            return await SendMessageAsync("getall", new TrafficDto(), "POST - MessageGetAll", "traffic-queue");
        }

        [HttpPost("event/getbyid/{id:long}")]
        public async Task<IActionResult> MessageGetByIdAsync(long id)
        {
            await LogTrafficAsync("POST - MessageGetByIdAsync", "Request");
            return await SendMessageAsync("getbyid", new TrafficDto { Id = id }, "POST - MessageGetById", "traffic-queue");
        }

        [HttpPost("event/create")]
        public async Task<IActionResult> MessageCreateAsync([FromBody] TrafficDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await LogTrafficAsync("POST - MessageCreateAsync", "Request");
            return await SendMessageAsync("create", dto, "POST - MessageCreate", "traffic-queue");
        }

        [HttpPut("event/update/{id:long}")]
        public async Task<IActionResult> MessageUpdateAsync(long id, [FromBody] TrafficDto dto)
        {
            if (!ModelState.IsValid || id != dto.Id)
                return BadRequest("Invalid payload or ID mismatch.");

            var existing = await _service.GetByIdAsync(id);
            if (existing is null)
                return NotFound();

            await LogTrafficAsync("PUT - MessageUpdate", "Request");
            return await SendMessageAsync("update", dto, "PUT - MessageUpdate", "traffic-queue");
        }

        [HttpGet("event/result/{id:long}")]
        public async Task<IActionResult> GetResultAsync(long id)
        {
            return await GetResultAsyncInternal(id, (action, messageResponse) =>
            {
                return action.ToLowerInvariant() switch
                {
                    "getall" => JsonSerializer.Deserialize<IEnumerable<TrafficDto>>(messageResponse),
                    "getbyid" => JsonSerializer.Deserialize<TrafficDto>(messageResponse),
                    "create" or "update" or "delete" => messageResponse,
                    _ => null
                };
            });
        }

        [HttpDelete("event/delete/{id:long}")]
        public async Task<IActionResult> MessageDeleteAsync(long id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing is null)
                return NotFound();

            await LogTrafficAsync("DELETE - MessageDeleteAsync", "Request");
            return await SendMessageAsync("delete", new TrafficDto { Id = id }, "DELETE - MessageDelete", "traffic-queue");
        }

        #endregion

        #region Additional Methods  

        #endregion

    }
}
