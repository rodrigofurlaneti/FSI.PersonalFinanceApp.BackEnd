using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Application.Messaging;
using Microsoft.AspNetCore.Mvc;

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

        public ExpenseCategoryControllerAsync(IExpenseCategoryAppService service, ITrafficAppService serviceTraffic, ILogger<ExpenseCategoryControllerAsync> logger,
            IMessageQueuePublisher publisher)
        {
            _service = service;
            _serviceTraffic = serviceTraffic;
            _logger = logger;
            _publisher = publisher;
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

                _publisher.Publish(envelope, "expense-category-queue");

                _logger.LogInformation("Mensagem enviada para fila com ação CREATE");

                await LogTraffic("POST - Create - ExpenseCategory - Async", "Response");

                return Accepted(new { message = "Mensagem enviada para processamento assíncrono" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enfileirar categoria de despesa");
                return StatusCode(500, "Erro interno ao processar a solicitação");
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

                await _service.UpdateAsync(dto);

                await LogTraffic("PUT - Update - ExpenseCategory - Async", "Response");

                _logger.LogInformation("Expense category with id {ExpenseCategoryId} updated successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error updating expense category with id {ExpenseCategoryId}", id);
                return StatusCode(500, "Error processing request");
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

                await _service.DeleteAsync(existingExpenseCategory);

                await LogTraffic("DELETE - Delete - ExpenseCategory - Async", "Response");

                _logger.LogInformation("Expense category with id {ExpenseCategoryId} deleted successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting expense category with id {ExpenseCategoryId}", id);
                return StatusCode(500, "Error processing request");
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
