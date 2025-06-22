using Azure.Messaging;
using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Application.Messaging;
using FSI.PersonalFinanceApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
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

        #region CRUD Operations

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                await LogTraffic("GET - GetAll - ExpenseCategory - Async", "Request");

                var result = await _service.GetAllAsync();

                await LogTraffic("GET - GetAll - ExpenseCategory - Async", "Response");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting expenses categories");
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                await LogTraffic("GET - GetById - ExpenseCategory - Async", "Request");

                var result = await _service.GetByIdAsync(id);

                await LogTraffic("GET - GetById - ExpenseCategory - Async", "Response");

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
        public async Task<IActionResult> Create([FromBody] ExpenseCategoryDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await LogTraffic("POST - Create - ExpenseCategory - Async", "Request");

                var envelope = new ExpenseCategoryMessage
                {
                    Action = "create",
                    Payload = dto
                };

                // Publica no RabbitMQ
                _publisher.Publish(envelope, "expense-category-queue");
                _logger.LogInformation("Message sent to queue with CREATE action");

                // Grava no banco após publicação com sucesso
                string messageContent = JsonSerializer.Serialize(envelope);
                MessagingDto messagingDto = new MessagingDto("Create", "expense-category-queue", messageContent, false, string.Empty);
                await _messagingAppService.AddAsync(messagingDto);

                await LogTraffic("POST - Create - ExpenseCategory - Async", "Response");

                return Accepted(new { message = "Message sent for asynchronous processing." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error queuing expense category.");
                return StatusCode(500, "Internal error processing request.");
            }
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] ExpenseCategoryDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for expense category update: {@ExpenseCategoryDto}", dto);
                    return BadRequest(ModelState);
                }

                if (id != dto.Id)
                {
                    _logger.LogWarning("Expense ID mismatch: route id = {RouteId}, dto id = {DtoId}", id, dto.Id);
                    return BadRequest("ID mismatch");
                }

                await LogTraffic("PUT - Update - ExpenseCategory - Async", "Request");

                var existingExpenseCategory = await _service.GetByIdAsync(id);
                if (existingExpenseCategory is null)
                {
                    _logger.LogWarning("Expense category with id {ExpenseCategoryId} not found for update", id);
                    return NotFound();
                }

                var envelope = new ExpenseCategoryMessage
                {
                    Action = "update",
                    Payload = dto
                };

                // Publica no RabbitMQ
                _publisher.Publish(envelope, "expense-category-queue");
                _logger.LogInformation("Message sent to queue with UPDATE action");

                // Grava no banco após publicação com sucesso
                string messageContent = JsonSerializer.Serialize(envelope);
                MessagingDto messagingDto = new MessagingDto("Update", "expense-category-queue", messageContent, false, string.Empty);
                await _messagingAppService.AddAsync(messagingDto);

                await LogTraffic("PUT - Update - ExpenseCategory - Async", "Response");

                return Accepted(new { message = "Message sent for asynchronous processing." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error queuing expense category update with id {ExpenseCategoryId}", id);
                return StatusCode(500, "Internal error processing the request.");
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await LogTraffic("DELETE - Delete - ExpenseCategory - Async", "Request");

                var existingExpenseCategory = await _service.GetByIdAsync(id);
                if (existingExpenseCategory is null)
                {
                    _logger.LogWarning("Expense category with id {ExpenseCategoryId} not found for deletion", id);
                    return NotFound();
                }

                // Cria o envelope da mensagem
                var envelope = new ExpenseCategoryMessage
                {
                    Action = "delete",
                    Payload = new ExpenseCategoryDto
                    {
                        Id = id
                    }
                };

                // Publica no RabbitMQ
                _publisher.Publish(envelope, "expense-category-queue");
                _logger.LogInformation("Message sent to queue with DELETE action");

                // Persiste no banco de dados
                string messageContent = JsonSerializer.Serialize(envelope);
                MessagingDto messagingDto = new MessagingDto("Delete", "expense-category-queue", messageContent, false, string.Empty);
                await _messagingAppService.AddAsync(messagingDto);

                await LogTraffic("DELETE - Delete - ExpenseCategory - Async", "Response");

                return Accepted(new { message = "Message sent for asynchronous processing." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error queuing deletion of expense category with id {ExpenseCategoryId}", id);
                return StatusCode(500, "Internal error processing the request.");
            }
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetAllFiltered([FromQuery] string filterBy, [FromQuery] string value)
        {
            try
            {
                await LogTraffic("GET - GetAllFiltered - ExpenseCategory - Async", "Request");

                var result = await _service.GetAllFilteredAsync(filterBy, value);

                await LogTraffic("GET - GetAllFiltered - ExpenseCategory - Async", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering expenses category by {FilterBy} ", filterBy);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("ordered")]
        public async Task<IActionResult> GetAllOrdered([FromQuery] string orderBy, [FromQuery] string direction = "asc")
        {
            try
            {
                await LogTraffic("GET - GetAllOrdered - ExpenseCategory - Async", "Request");

                var result = await _service.GetAllOrderedAsync(orderBy, direction);

                await LogTraffic("GET - GetAllOrdered - ExpenseCategory - Async", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ordering expenses categories by {OrderBy} {Direction}", orderBy, direction);
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
            await _serviceTraffic.AddAsync(dto);
        }

        #endregion
    }
}
