using FSI.PersonalFinanceApp.Api.Controllers.Base;
using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Application.Messaging;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/incomes/async")]
    public class IncomeControllerAsync : BaseAsyncController<IncomeDto>
    {
        private readonly IIncomeAppService _service;

        public IncomeControllerAsync(IIncomeAppService service, ITrafficAppService trafficService, ILogger<IncomeControllerAsync> logger,
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
                await LogTrafficAsync("GET - GetAll - Income - Async", "Request");

                var result = await _service.GetAllAsync();

                await LogTrafficAsync("GET - GetAll - Income - Async", "Response");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting income");
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                await LogTrafficAsync("GET - GetById - Income - Async", "Request");

                var result = await _service.GetByIdAsync(id);

                await LogTrafficAsync("GET - GetById - Income - Async", "Response");

                if (result is null)
                {
                    _logger.LogWarning("Income with id {IncomeId} not found", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving income with id {IncomeId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IncomeDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for income creation: {@IncomeDto}", dto);
                    return BadRequest(ModelState);
                }

                await LogTrafficAsync("POST - Create - Income - Async", "Request");

                await _service.AddAsync(dto);

                await LogTrafficAsync("POST - Create - Income - Async", "Response");

                _logger.LogInformation("Income created with id {IncomeId}", dto.Id);

                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating income: {@IncomeDto}", dto);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] IncomeDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for income update: {@IncomeDto}", dto);
                    return BadRequest(ModelState);
                }

                if (id != dto.Id)
                {
                    _logger.LogWarning("Income ID mismatch: route id = {RouteId}, dto id = {DtoId}", id, dto.Id);
                    return BadRequest("ID mismatch");
                }

                await LogTrafficAsync("PUT - Update - Income - Async", "Request");

                var existingIncome = await _service.GetByIdAsync(id);
                if (existingIncome is null)
                {
                    _logger.LogWarning("Income with id {IncomeId} not found for update", id);
                    return NotFound();
                }

                await _service.UpdateAsync(dto);

                await LogTrafficAsync("PUT - Update - Income - Async", "Response");

                _logger.LogInformation("Income with id {IncomeId} updated successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating income with id {IncomeId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await LogTrafficAsync("DELETE - Delete - Income - Async", "Request");

                var existingIncome = await _service.GetByIdAsync(id);
                if (existingIncome is null)
                {
                    _logger.LogWarning("Income with id {IncomeId} not found for deletion", id);
                    return NotFound();
                }

                await _service.DeleteAsync(existingIncome);

                await LogTrafficAsync("DELETE - Delete - Income - Async", "Response");

                _logger.LogInformation("Income with id {IncomeId} deleted successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting income with id {IncomeId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetAllFiltered([FromQuery] string filterBy, [FromQuery] string value)
        {
            try
            {
                await LogTrafficAsync("GET - GetAllFiltered - Income - Async", "Request");

                var result = await _service.GetAllFilteredAsync(filterBy, value);

                await LogTrafficAsync("GET - GetAllFiltered - Income - Async", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering income by {FilterBy} ", filterBy);
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
            return await SendMessageAsync("getall", new IncomeDto(), "POST - MessageGetAll", "income-queue");
        }

        [HttpPost("event/getbyid/{id:long}")]
        public async Task<IActionResult> MessageGetByIdAsync(long id)
        {
            await LogTrafficAsync("POST - MessageGetByIdAsync", "Request");
            return await SendMessageAsync("getbyid", new IncomeDto { Id = id }, "POST - MessageGetById", "income-queue");
        }

        [HttpPost("event/create")]
        public async Task<IActionResult> MessageCreateAsync([FromBody] IncomeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await LogTrafficAsync("POST - MessageCreateAsync", "Request");
            return await SendMessageAsync("create", dto, "POST - MessageCreate", "income-queue");
        }

        [HttpPut("event/update/{id:long}")]
        public async Task<IActionResult> MessageUpdateAsync(long id, [FromBody] IncomeDto dto)
        {
            if (!ModelState.IsValid || id != dto.Id)
                return BadRequest("Invalid payload or ID mismatch.");

            var existing = await _service.GetByIdAsync(id);
            if (existing is null)
                return NotFound();

            await LogTrafficAsync("PUT - MessageUpdate", "Request");
            return await SendMessageAsync("update", dto, "PUT - MessageUpdate", "income-queue");
        }

        [HttpGet("event/result/{id:long}")]
        public async Task<IActionResult> GetResultAsync(long id)
        {
            return await GetResultAsyncInternal(id, (action, messageResponse) =>
            {
                return action.ToLowerInvariant() switch
                {
                    "getall" => JsonSerializer.Deserialize<IEnumerable<IncomeDto>>(messageResponse),
                    "getbyid" => JsonSerializer.Deserialize<IncomeDto>(messageResponse),
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
            return await SendMessageAsync("delete", new IncomeDto { Id = id }, "DELETE - MessageDelete", "income-queue");
        }

        #endregion

        #region Additional Methods  

        #endregion
    }
}
