using Azure.Messaging;
using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Application.Messaging;
using FSI.PersonalFinanceApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/expense-categories/async")]
    public class ExpenseCategoryControllerAsync : ControllerBase
    {
        private readonly IExpenseCategoryAppService _service;
        private readonly ITrafficAppService _serviceTraffic;
        private readonly ILogger<ExpenseCategoryControllerAsync> _logger;
        private readonly IMessageQueuePublisher _publisher;
        private readonly IMessagingAppService _messagingAppService;

        public ExpenseCategoryControllerAsync(IExpenseCategoryAppService service, ITrafficAppService serviceTraffic, ILogger<ExpenseCategoryControllerAsync> logger,
            IMessageQueuePublisher publisher, IMessagingAppService messagingAppService)
        {
            _service = service;
            _serviceTraffic = serviceTraffic;
            _logger = logger;
            _publisher = publisher;
            _messagingAppService = messagingAppService;
        }

        #region CRUD Operations Async

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                await LogTrafficAsync("GET - GetAll - ExpenseCategory - Async", "Request");

                var result = await _service.GetAllAsync();

                await LogTrafficAsync("GET - GetAll - ExpenseCategory - Async", "Response");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting expenses category");
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            try
            {
                await LogTrafficAsync("GET - GetById - ExpenseCategory - Async", "Request");

                var result = await _service.GetByIdAsync(id);

                await LogTrafficAsync("GET - GetById - ExpenseCategory - Async", "Response");

                if (result is null)
                {
                    _logger.LogWarning("Expense with id {ExpenseCategoryId} not found", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving expense category with id {ExpenseCategoryId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ExpenseCategoryDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for expense category creation: {@ExpenseDto}", dto);
                    return BadRequest(ModelState);
                }

                await LogTrafficAsync("POST - CreateAsync - ExpenseCategory - Async", "Request");

                var id = await _service.AddAsync(dto);

                await LogTrafficAsync("POST - CreateAsync - ExpenseCategory - Async", "Response");

                dto.Id = id;

                _logger.LogInformation("Expense category created with id {ExpenseId}", id);

                var url = Url.Action(nameof(GetByIdAsync), "ExpenseCategory", new { id = id });

                return Created(url, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating expense category: {@ExpenseCategoryDto}", dto);
                return StatusCode(500, "Error processing request");
            }
        }


        [HttpPut("update/{id:long}")]
        public async Task<IActionResult> UpdateAsync(long id, [FromBody] ExpenseCategoryDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for expense update: {@ExpenseDto}", dto);
                    return BadRequest(ModelState);
                }

                if (id != dto.Id)
                {
                    _logger.LogWarning("Expense ID mismatch: route id = {RouteId}, dto id = {DtoId}", id, dto.Id);
                    return BadRequest("ID mismatch");
                }

                await LogTrafficAsync("PUT - Update - Expense - Async", "Request");

                var existingExpense = await _service.GetByIdAsync(id);
                if (existingExpense is null)
                {
                    _logger.LogWarning("Expense with id {ExpenseId} not found for update", id);
                    return NotFound();
                }

                await _service.UpdateAsync(dto);

                await LogTrafficAsync("PUT - Update - Expense - Async", "Response");

                _logger.LogInformation("Expense with id {ExpenseId} updated successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating expense with id {ExpenseId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpDelete("delete/{id:long}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            try
            {
                await LogTrafficAsync("DELETE - Delete - Expense - Async", "Request");

                var existingExpense = await _service.GetByIdAsync(id);
                if (existingExpense is null)
                {
                    _logger.LogWarning("Expense with id {ExpenseId} not found for deletion", id);
                    return NotFound();
                }

                await _service.DeleteAsync(existingExpense);

                await LogTrafficAsync("DELETE - Delete - Expense - Async", "Response");

                _logger.LogInformation("Expense with id {ExpenseId} deleted successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting expense with id {ExpenseId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetAllFilteredAsync([FromQuery] string filterBy, [FromQuery] string value)
        {
            try
            {
                await LogTrafficAsync("GET - GetAllFiltered - ExpenseCategory - Async", "Request");

                var result = await _service.GetAllFilteredAsync(filterBy, value);

                await LogTrafficAsync("GET - GetAllFiltered - ExpenseCategory - Async", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering expenses category by {FilterBy} ", filterBy);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("ordered")]
        public async Task<IActionResult> GetAllOrderedAsync([FromQuery] string orderBy, [FromQuery] string direction = "asc")
        {
            try
            {
                await LogTrafficAsync("GET - GetAllOrdered - ExpenseCategory - Async", "Request");

                var result = await _service.GetAllOrderedAsync(orderBy, direction);

                await LogTrafficAsync("GET - GetAllOrdered - ExpenseCategory - Async", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ordering expenses categories by {OrderBy} {Direction}", orderBy, direction);
                return StatusCode(500, "Error processing request");
            }
        }

        #endregion

        #region CRUD Operations Async - Event Driven Architecture - Request response via polling - Async Message Dispatch with Deferred Response

        [HttpPost("event/getall")]
        public async Task<IActionResult> MessageGetAllAsync()
        {
            await LogTrafficAsync("POST - MessageGetAllAsync", "Request");
            return await SendMessageAsync("getall", new ExpenseCategoryDto(), "POST - MessageGetAll");
        }

        [HttpPost("event/getbyid/{id:long}")]
        public async Task<IActionResult> MessageGetByIdAsync(long id)
        {
            await LogTrafficAsync("POST - MessageGetByIdAsync", "Request");
            return await SendMessageAsync("getbyid", new ExpenseCategoryDto { Id = id }, "POST - MessageGetById");
        }

        [HttpPost("event/create")]
        public async Task<IActionResult> MessageCreateAsync([FromBody] ExpenseCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await LogTrafficAsync("POST - MessageCreateAsync", "Request");
            return await SendMessageAsync("create", dto, "POST - MessageCreate");
        }

        [HttpPut("event/update/{id:long}")]
        public async Task<IActionResult> MessageUpdateAsync(long id, [FromBody] ExpenseCategoryDto dto)
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
                        response = JsonSerializer.Deserialize<IEnumerable<ExpenseCategoryDto>>(result.MessageResponse);
                        break;

                    case "create":
                    case "update":
                    case "delete":
                        response = result.MessageResponse;
                        break;

                    case "getbyid":
                        response = JsonSerializer.Deserialize<ExpenseCategoryDto>(result.MessageResponse);
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

        [HttpDelete("event/delete/{id:long}")]
        public async Task<IActionResult> MessageDeleteAsync(long id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing is null)
                return NotFound();

            await LogTrafficAsync("DELETE - MessageDeleteAsync", "Request");
            return await SendMessageAsync("delete", new ExpenseCategoryDto { Id = id }, "DELETE - MessageDelete");
        }

        #endregion

        #region Additional Methods

        #endregion

        #region Additional Methods Private 

        private async Task<IActionResult> SendMessageAsync(string action, ExpenseCategoryDto payload, string logPrefix)
        {
            var envelope = new ExpenseCategoryMessage
            {
                Action = action,
                Payload = payload,
                MessagingId = 0
            };

            var messageRequest = JsonSerializer.Serialize(envelope);

            var idMessaging = await _messagingAppService.AddAsync(new MessagingDto(
                action,
                "expense-category-queue",
                messageRequest,
                false,
                string.Empty
            ));

            envelope.MessagingId = idMessaging;
            _publisher.Publish(envelope, "expense-category-queue");

            _logger.LogInformation("📤 '{Action}' message sent to queue, ID {Id}", action, idMessaging);

            await LogTrafficAsync($"{logPrefix} - ExpenseCategory - Async", "Response");

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
