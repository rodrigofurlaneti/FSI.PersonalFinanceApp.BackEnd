using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Application.Messaging;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FSI.PersonalFinanceApp.Api.ControllersBase
{
    public abstract class AsyncEventControllerBase<TDto, TEnvelope> : ControllerBase, IAsyncEventController<TDto>
        where TDto : class, new()
        where TEnvelope : IMessageEnvelope<TDto>, new()
    {
        protected readonly ILogger _logger;
        protected readonly IMessageQueuePublisher _publisher;
        protected readonly IMessagingAppService _messagingService;
        protected readonly ITrafficAppService _trafficService;
        protected readonly string _queueName;

        protected AsyncEventControllerBase(
            ILogger logger,
            IMessageQueuePublisher publisher,
            IMessagingAppService messagingService,
            ITrafficAppService trafficService,
            string queueName)
        {
            _logger = logger;
            _publisher = publisher;
            _messagingService = messagingService;
            _trafficService = trafficService;
            _queueName = queueName;
        }

        protected virtual async Task<IActionResult> SendMessageAsync(string action, TDto payload, string logPrefix)
        {
            var envelope = new TEnvelope
            {
                Action = action,
                Payload = payload,
                MessagingId = 0
            };

            var messageRequest = JsonSerializer.Serialize(envelope);

            var idMessaging = await _messagingService.AddAsync(new MessagingDto(
                action,
                _queueName,
                messageRequest,
                false,
                string.Empty
            ));

            envelope.MessagingId = idMessaging;
            _publisher.Publish(envelope, _queueName);

            _logger.LogInformation("📤 '{Action}' message sent to queue, ID {Id}", action, idMessaging);

            await LogTrafficAsync($"{logPrefix} - AsyncEvent", "Response");

            return Accepted(new { message = "Request queued successfully", id = idMessaging });
        }

        protected virtual async Task LogTrafficAsync(string method, string action)
        {
            var dto = new TrafficDto(method, action, DateTime.Now);
            await _trafficService.AddAsync(dto);
        }

        public virtual async Task<IActionResult> MessageCreateAsync([FromBody] TDto dto)
            => await SendMessageAsync("create", dto, "POST - MessageCreate");

        public virtual async Task<IActionResult> MessageUpdateAsync(long id, [FromBody] TDto dto)
            => await SendMessageAsync("update", dto, "PUT - MessageUpdate");

        public virtual async Task<IActionResult> MessageDeleteAsync(long id)
            => await SendMessageAsync("delete", new TDto { }, "DELETE - MessageDelete");

        public virtual async Task<IActionResult> MessageGetByIdAsync(long id)
            => await SendMessageAsync("getbyid", new TDto { }, "POST - MessageGetById");

        public virtual async Task<IActionResult> MessageGetAllAsync()
            => await SendMessageAsync("getall", new TDto(), "POST - MessageGetAll");

        public virtual async Task<IActionResult> GetResultAsync(long id)
        {
            var result = await _messagingService.GetByIdAsync(id);
            if (result == null)
                return NotFound("Message not found.");

            if (!result.IsProcessed)
                return Accepted(new { message = "Still in processing", id });

            object? response;
            switch (result.Action.ToLowerInvariant())
            {
                case "getall":
                    response = JsonSerializer.Deserialize<IEnumerable<TDto>>(result.MessageResponse);
                    break;

                case "create":
                case "update":
                case "delete":
                    response = string.Empty;
                    break;

                case "getbyid":
                    response = JsonSerializer.Deserialize<TDto>(result.MessageResponse);
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
    }
}
