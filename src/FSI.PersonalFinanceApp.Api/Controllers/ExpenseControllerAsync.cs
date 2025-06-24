using FSI.PersonalFinanceApp.Api.Controllers.Base;
using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Application.Messaging;
using FSI.PersonalFinanceApp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/expenses/async")]
    public class ExpenseControllerAsync : BaseAsyncController<ExpenseDto>
    {
        private readonly IExpenseAppService _service;
        private readonly IMessagingAppService _messagingAppService;

        public ExpenseControllerAsync(IExpenseAppService service, ITrafficAppService trafficService, ILogger<ExpenseControllerAsync> logger,
            IMessageQueuePublisher publisher, IMessagingAppService messagingService) : base(logger, publisher, messagingService, trafficService)
        {
            _service = service;
            _messagingAppService = messagingService;
        }

        #region CRUD Operations

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                await LogTrafficAsync("GET - GetAll - Expense - Async", "Request");

                var result = await _service.GetAllAsync();

                await LogTrafficAsync("GET - GetAll - Expense - Async", "Response");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting expenses");
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                await LogTrafficAsync("GET - GetById - Expense - Async", "Request");

                var result = await _service.GetByIdAsync(id);

                await LogTrafficAsync("GET - GetById - Expense - Async", "Response");

                if (result is null)
                {
                    _logger.LogWarning("Expense with id {ExpenseId} not found", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving expense with id {ExpenseId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExpenseDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for expense creation: {@ExpenseDto}", dto);
                    return BadRequest(ModelState);
                }

                await LogTrafficAsync("POST - Create - Expense - Async", "Request");

                await _service.AddAsync(dto);

                await LogTrafficAsync("POST - Create - Expense - Async", "Response");

                _logger.LogInformation("Expense created with id {ExpenseId}", dto.Id);

                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating expense: {@ExpenseDto}", dto);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] ExpenseDto dto)
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

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
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
        public async Task<IActionResult> GetAllFiltered([FromQuery] string filterBy, [FromQuery] string value)
        {
            try
            {
                await LogTrafficAsync("GET - GetAllFiltered - Expense - Async", "Request");

                var result = await _service.GetAllFilteredAsync(filterBy, value);

                await LogTrafficAsync("GET - GetAllFiltered - Expense - Async", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering expenses by {FilterBy} ", filterBy);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("ordered")]
        public async Task<IActionResult> GetAllOrdered([FromQuery] string orderBy, [FromQuery] string direction = "asc")
        {
            try
            {
                await LogTrafficAsync("GET - GetAllOrdered - Expense - Async", "Request");

                var result = await _service.GetAllOrderedAsync(orderBy, direction);

                await LogTrafficAsync("GET - GetAllOrdered - Expense - Async", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ordering expenses by {OrderBy} {Direction}", orderBy, direction);
                return StatusCode(500, "Error processing request");
            }
        }



        #endregion

        #region CRUD Operations Async - Event Driven Architecture - Request response via polling - Async Message Dispatch with Deferred Response

        [HttpPost("event/getall")]
        public async Task<IActionResult> MessageGetAllAsync()
        {
            await LogTrafficAsync("POST - MessageGetAllAsync", "Request");
            return await SendMessageAsync("getall", new ExpenseDto(), "POST - MessageGetAll", "expense-queue");
        }

        [HttpPost("event/getbyid/{id:long}")]
        public async Task<IActionResult> MessageGetByIdAsync(long id)
        {
            await LogTrafficAsync("POST - MessageGetByIdAsync", "Request");
            return await SendMessageAsync("getbyid", new ExpenseDto { Id = id }, "POST - MessageGetById", "expense-queue");
        }

        [HttpPost("event/create")]
        public async Task<IActionResult> MessageCreateAsync([FromBody] ExpenseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await LogTrafficAsync("POST - MessageCreateAsync", "Request");
            return await SendMessageAsync("create", dto, "POST - MessageCreate", "expense-queue");
        }

        [HttpPut("event/update/{id:long}")]
        public async Task<IActionResult> MessageUpdateAsync(long id, [FromBody] ExpenseDto dto)
        {
            if (!ModelState.IsValid || id != dto.Id)
                return BadRequest("Invalid payload or ID mismatch.");

            var existing = await _service.GetByIdAsync(id);
            if (existing is null)
                return NotFound();

            await LogTrafficAsync("PUT - MessageUpdate", "Request");
            return await SendMessageAsync("update", dto, "PUT - MessageUpdate", "expense-queue");
        }

        [HttpGet("event/result/{id:long}")]
        public async Task<IActionResult> GetResultAsync(long id)
        {
            return await GetResultAsyncInternal(id, (action, messageResponse) =>
            {
                return action.ToLowerInvariant() switch
                {
                    "getall" => JsonSerializer.Deserialize<IEnumerable<ExpenseDto>>(messageResponse),
                    "getbyid" => JsonSerializer.Deserialize<ExpenseDto>(messageResponse),
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
            return await SendMessageAsync("delete", new ExpenseDto { Id = id }, "DELETE - MessageDelete", "expense-queue");
        }

        #endregion

        #region Additional Methods  

        #endregion
    }
}
