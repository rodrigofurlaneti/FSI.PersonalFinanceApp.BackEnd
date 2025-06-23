using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Application.Messaging;
using FSI.PersonalFinanceApp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/financial-goals/async")]
    public class FinancialGoalControllerAsync : ControllerBase
    {
        private readonly IFinancialGoalAppService _service;
        private readonly ITrafficAppService _serviceTraffic;
        private readonly ILogger<FinancialGoalControllerSync> _logger;
        private readonly IMessageQueuePublisher _publisher;
        private readonly IMessagingAppService _messagingAppService;

        public FinancialGoalControllerAsync(IFinancialGoalAppService service, ITrafficAppService serviceTraffic, ILogger<FinancialGoalControllerSync> logger,
            IMessageQueuePublisher publisher, IMessagingAppService messagingAppService)
        {
            _service = service;
            _serviceTraffic = serviceTraffic;
            _logger = logger;
            _publisher = publisher;
            _messagingAppService = messagingAppService;
        }

        #region CRUD Operations

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                await LogTrafficAsync("GET - GetAll - FinancialGoal - Async", "Request");

                var result = await _service.GetAllAsync();

                await LogTrafficAsync("GET - GetAll - FinancialGoal - Async", "Response");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting financial goal");
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                await LogTrafficAsync("GET - GetById - FinancialGoal - Async", "Request");

                var result = await _service.GetByIdAsync(id);

                await LogTrafficAsync("GET - GetById - FinancialGoal - Async", "Response");

                if (result is null)
                {
                    _logger.LogWarning("Financial goal with id {FinancialGoalId} not found", id);
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
        public async Task<IActionResult> Create([FromBody] FinancialGoalDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for financial goal creation: {@FinancialGoalDto}", dto);
                    return BadRequest(ModelState);
                }

                await LogTrafficAsync("POST - Create - FinancialGoal - Async", "Request");

                await _service.AddAsync(dto);

                await LogTrafficAsync("POST - Create - FinancialGoal - Async", "Response");

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
        public async Task<IActionResult> Update(long id, [FromBody] FinancialGoalDto dto)
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

                await LogTrafficAsync("PUT - Update - FinancialGoal - Async", "Request");

                var existingFinancialGoal = await _service.GetByIdAsync(id);
                if (existingFinancialGoal is null)
                {
                    _logger.LogWarning("Financial goal with id {FinancialGoalId} not found for update", id);
                    return NotFound();
                }

                await _service.UpdateAsync(dto);

                await LogTrafficAsync("PUT - Update - FinancialGoal - Async", "Response");

                _logger.LogInformation("Financial goal with id {FinancialGoalId} updated successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating financial goal with id {FinancialGoalId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await LogTrafficAsync("DELETE - Delete - FinancialGoal - Async", "Request");

                var existingFinancialGoal = await _service.GetByIdAsync(id);
                if (existingFinancialGoal is null)
                {
                    _logger.LogWarning("FinancialGoal with id {FinancialGoalId} not found for deletion", id);
                    return NotFound();
                }

                await _service.DeleteAsync(existingFinancialGoal);

                await LogTrafficAsync("DELETE - Delete - FinancialGoal - Async", "Response");

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
        public async Task<IActionResult> GetAllFiltered([FromQuery] string filterBy, [FromQuery] string value)
        {
            try
            {
                await LogTrafficAsync("GET - GetAllFiltered - FinancialGoal - Async", "Request");

                var result = await _service.GetAllFilteredAsync(filterBy, value);

                await LogTrafficAsync("GET - GetAllFiltered - FinancialGoal - Async", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering financial goal by {FilterBy} ", filterBy);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("ordered")]
        public async Task<IActionResult> GetAllOrdered([FromQuery] string orderBy, [FromQuery] string direction = "asc")
        {
            try
            {
                await LogTrafficAsync("GET - GetAllOrdered - FinancialGoal - Async", "Request");

                var result = await _service.GetAllOrderedAsync(orderBy, direction);

                await LogTrafficAsync("GET - GetAllOrdered - FinancialGoal - Async", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ordering financial goal by {OrderBy} {Direction}", orderBy, direction);
                return StatusCode(500, "Error processing request");
            }
        }

        #endregion

        #region CRUD Operations Async - Event Driven Architecture - Request response via polling - Async Message Dispatch with Deferred Response

        [HttpPost("event/getall")]
        public async Task<IActionResult> MessageGetAllAsync()
        {
            await LogTrafficAsync("POST - MessageGetAllAsync", "Request");
            return await SendMessageAsync("getall", new FinancialGoalDto(), "POST - MessageGetAll");
        }

        [HttpPost("event/getbyid/{id:long}")]
        public async Task<IActionResult> MessageGetByIdAsync(long id)
        {
            await LogTrafficAsync("POST - MessageGetByIdAsync", "Request");
            return await SendMessageAsync("getbyid", new FinancialGoalDto { Id = id }, "POST - MessageGetById");
        }

        [HttpPost("event/create")]
        public async Task<IActionResult> MessageCreateAsync([FromBody] FinancialGoalDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await LogTrafficAsync("POST - MessageCreateAsync", "Request");
            return await SendMessageAsync("create", dto, "POST - MessageCreate");
        }

        [HttpPut("event/update/{id:long}")]
        public async Task<IActionResult> MessageUpdateAsync(long id, [FromBody] FinancialGoalDto dto)
        {
            if (!ModelState.IsValid || id != dto.Id)
                return BadRequest("Invalid payload or ID mismatch.");

            var existing = await _service.GetByIdAsync(id);
            if (existing is null)
                return NotFound();

            await LogTrafficAsync("PUT - MessageUpdate", "Request");
            return await SendMessageAsync("update", dto, "PUT - MessageUpdate");
        }

        [HttpGet("event/result/{id:long}")]
        public async Task<IActionResult> GetResultAsync(long id)
        {
            try
            {
                var result = await _messagingAppService.GetByIdAsync(id);

                if (result == null)
                    return NotFound("Message not found.");

                if (!result.IsProcessed)
                    return Accepted(new { message = "Still in processing", id });

                object? response;

                // Determina como desserializar com base na ação original
                switch (result.Action.ToLowerInvariant())
                {
                    case "getall":
                        response = JsonSerializer.Deserialize<IEnumerable<FinancialGoalDto>>(result.MessageResponse);
                        break;

                    case "create":
                    case "update":
                    case "delete":
                        response = result.MessageResponse;
                        break;

                    case "getbyid":
                        response = JsonSerializer.Deserialize<FinancialGoalDto>(result.MessageResponse);
                        break;

                    default:
                        _logger.LogWarning("Unknown action '{Action}' in result ID {Id}", result.Action, id);
                        return BadRequest("Unknown action type.");
                }

                return Ok(new
                {
                    id = result.Id,
                    originalAction = result.Action,
                    processed = result.IsProcessed,
                    response
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error querying message ID result {MessagingId}", id);
                return StatusCode(500, "Error getting message result.");
            }
        }


        #endregion

        #region Additional Methods
        // Add any additional methods specific to financial goals here, if needed.
        #endregion

        #region Additional Methods Private 

        private async Task<IActionResult> SendMessageAsync(string action, FinancialGoalDto payload, string logPrefix)
        {
            var envelope = new FinancialGoalMessage
            {
                Action = action,
                Payload = payload,
                MessagingId = 0
            };

            var messageRequest = JsonSerializer.Serialize(envelope);

            var idMessaging = await _messagingAppService.AddAsync(new MessagingDto(
                action,
                "financial-goal-queue",
                messageRequest,
                false,
                string.Empty
            ));

            envelope.MessagingId = idMessaging;

            _publisher.Publish(envelope, "financial-goal-queue");

            _logger.LogInformation("📤 '{Action}' message sent to queue, ID {Id}", action, idMessaging);

            await LogTrafficAsync($"{logPrefix} - FinancialGoal - Async", "Response");

            return Accepted(new { message = "Request queued successfully", id = idMessaging });
        }


        private async Task LogTrafficAsync(string method, string action)
        {
            var dto = new TrafficDto(method, action, DateTime.Now);
            await _serviceTraffic.AddAsync(dto);
        }

        #endregion
    }
}
