using FSI.PersonalFinanceApp.Api.Controllers.Base;
using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Application.Messaging;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/accounts/async")]
    public class AccountControllerAsync : BaseAsyncController<AccountDto>
    {
        private readonly IAccountAppService _service;

        public AccountControllerAsync(IAccountAppService service, ITrafficAppService trafficService, ILogger<AccountControllerAsync> logger,
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
                await LogTrafficAsync("GET - GetAll - Account - Async", "Request");

                var result = await _service.GetAllAsync();

                await LogTrafficAsync("GET - GetAll - Account - Async", "Response");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting account");
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                await LogTrafficAsync("GET - GetById - Account - Async", "Request");

                var result = await _service.GetByIdAsync(id);

                await LogTrafficAsync("GET - GetById - Account - Async", "Response");

                if (result is null)
                {
                    _logger.LogWarning("Account with id {AccountId} not found", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving account with id {AccountId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccountDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for expense creation: {@AccountDto}", dto);
                    return BadRequest(ModelState);
                }

                await LogTrafficAsync("POST - Create - Account - Async", "Request");

                await _service.AddAsync(dto);

                await LogTrafficAsync("POST - Create - Account - Async", "Response");

                _logger.LogInformation("Account created with id {AccountId}", dto.Id);

                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating account: {@AccountDto}", dto);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] AccountDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for account update: {@AccountDto}", dto);
                    return BadRequest(ModelState);
                }

                if (id != dto.Id)
                {
                    _logger.LogWarning("Account ID mismatch: route id = {RouteId}, dto id = {DtoId}", id, dto.Id);
                    return BadRequest("ID mismatch");
                }

                await LogTrafficAsync("PUT - Update - Account - Async", "Request");

                var existingAccount = await _service.GetByIdAsync(id);
                if (existingAccount is null)
                {
                    _logger.LogWarning("Account with id {AccountId} not found for update", id);
                    return NotFound();
                }

                await _service.UpdateAsync(dto);

                await LogTrafficAsync("PUT - Update - Account - Async", "Response");

                _logger.LogInformation("Account with id {AccountId} updated successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating account with id {AccountId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await LogTrafficAsync("DELETE - Delete - Account - Async", "Request");

                var existingAccount = await _service.GetByIdAsync(id);
                if (existingAccount is null)
                {
                    _logger.LogWarning("Account with id {AccountId} not found for deletion", id);
                    return NotFound();
                }

                await _service.DeleteAsync(existingAccount);

                await LogTrafficAsync("DELETE - Delete - Account - Async", "Response");

                _logger.LogInformation("Account with id {AccountId} deleted successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting account with id {AccountId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetAllFiltered([FromQuery] string filterBy, [FromQuery] string value)
        {
            try
            {
                await LogTrafficAsync("GET - GetAllFiltered - Account - Async", "Request");

                var result = await _service.GetAllFilteredAsync(filterBy, value);

                await LogTrafficAsync("GET - GetAllFiltered - Account - Async", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering account by {FilterBy} ", filterBy);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("ordered")]
        public async Task<IActionResult> GetAllOrdered([FromQuery] string orderBy, [FromQuery] string direction = "asc")
        {
            try
            {
                await LogTrafficAsync("GET - GetAllOrdered - Account - Async", "Request");

                var result = await _service.GetAllOrderedAsync(orderBy, direction);

                await LogTrafficAsync("GET - GetAllOrdered - Account - Async", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ordering account by {OrderBy} {Direction}", orderBy, direction);
                return StatusCode(500, "Error processing request");
            }
        }

        #endregion

        #region CRUD Operations Async - Event Driven Architecture - Request response via polling - Async Message Dispatch with Deferred Response

        [HttpPost("event/getall")]
        public async Task<IActionResult> MessageGetAllAsync()
        {
            await LogTrafficAsync("POST - MessageGetAllAsync", "Request");
            return await SendMessageAsync("getall", new AccountDto(), "POST - MessageGetAll", "account-queue");
        }

        [HttpPost("event/getbyid/{id:long}")]
        public async Task<IActionResult> MessageGetByIdAsync(long id)
        {
            await LogTrafficAsync("POST - MessageGetByIdAsync", "Request");
            return await SendMessageAsync("getbyid", new AccountDto { Id = id }, "POST - MessageGetById", "account-queue");
        }

        [HttpPost("event/create")]
        public async Task<IActionResult> MessageCreateAsync([FromBody] AccountDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await LogTrafficAsync("POST - MessageCreateAsync", "Request");
            return await SendMessageAsync("create", dto, "POST - MessageCreate", "account-queue");
        }

        [HttpPut("event/update/{id:long}")]
        public async Task<IActionResult> MessageUpdateAsync(long id, [FromBody] AccountDto dto)
        {
            if (!ModelState.IsValid || id != dto.Id)
                return BadRequest("Invalid payload or ID mismatch.");

            var existing = await _service.GetByIdAsync(id);
            if (existing is null)
                return NotFound();

            await LogTrafficAsync("PUT - MessageUpdate", "Request");
            return await SendMessageAsync("update", dto, "PUT - MessageUpdate", "account-queue");
        }

        [HttpGet("event/result/{id:long}")]
        public async Task<IActionResult> GetResultAsync(long id)
        {
            return await GetResultAsyncInternal(id, (action, messageResponse) =>
            {
                return action.ToLowerInvariant() switch
                {
                    "getall" => JsonSerializer.Deserialize<IEnumerable<AccountDto>>(messageResponse),
                    "getbyid" => JsonSerializer.Deserialize<AccountDto>(messageResponse),
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
            return await SendMessageAsync("delete", new AccountDto { Id = id }, "DELETE - MessageDelete", "account-queue");
        }

        #endregion

        #region Additional Methods

        #endregion

    }
}
