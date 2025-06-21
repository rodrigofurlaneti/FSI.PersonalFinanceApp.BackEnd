using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/transactions/async")]
    public class TransactionControllerAsync : ControllerBase
    {
        private readonly ITransactionAppService _service;
        private readonly ITrafficAppService _serviceTraffic;
        private readonly ILogger<TransactionControllerAsync> _logger;

        public TransactionControllerAsync(ITransactionAppService service, ITrafficAppService serviceTraffic, ILogger<TransactionControllerAsync> logger)
        {
            _service = service;
            _serviceTraffic = serviceTraffic;
            _logger = logger;
        }

        #region CRUD Operations

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                await LogTraffic("GET - GetAll - Transaction - Async", "Request");

                var result = await _service.GetAllAsync();

                await LogTraffic("GET - GetAll - Transaction - Async", "Response");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting transaction");
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                await LogTraffic("GET - GetById - Transaction - Async", "Request");

                var result = await _service.GetByIdAsync(id);

                await LogTraffic("GET - GetById - Transaction - Async", "Response");

                if (result is null)
                {
                    _logger.LogWarning("Transaction with id {TransactionId} not found", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transaction with id {TransactionId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TransactionDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for transaction creation: {@TransactionDto}", dto);
                    return BadRequest(ModelState);
                }

                await LogTraffic("POST - Create - Transaction - Async", "Request");

                await _service.AddAsync(dto);

                await LogTraffic("POST - Create - Transaction - Async", "Response");

                _logger.LogInformation("Transaction created with id {TransactionId}", dto.Id);

                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating transaction: {@TransactionDto}", dto);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] TransactionDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for transaction update: {@TransactionDto}", dto);
                    return BadRequest(ModelState);
                }

                if (id != dto.Id)
                {
                    _logger.LogWarning("Transaction ID mismatch: route id = {RouteId}, dto id = {DtoId}", id, dto.Id);
                    return BadRequest("ID mismatch");
                }

                await LogTraffic("PUT - Update - Transaction - Async", "Request");

                var existingTransaction = await _service.GetByIdAsync(id);
                if (existingTransaction is null)
                {
                    _logger.LogWarning("Transaction with id {TransactionId} not found for update", id);
                    return NotFound();
                }

                await _service.UpdateAsync(dto);

                await LogTraffic("PUT - Update - Transaction - Async", "Response");

                _logger.LogInformation("Transaction with id {TransactionId} updated successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating transaction with id {TransactionId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await LogTraffic("DELETE - Delete - Transaction - Async", "Request");

                var existingTransaction = await _service.GetByIdAsync(id);
                if (existingTransaction is null)
                {
                    _logger.LogWarning("Transaction with id {TransactionId} not found for deletion", id);
                    return NotFound();
                }

                await _service.DeleteAsync(existingTransaction);

                await LogTraffic("DELETE - Delete - Transaction - Async", "Response");

                _logger.LogInformation("Transaction with id {TransactionId} deleted successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting transaction with id {TransactionId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetAllFiltered([FromQuery] string filterBy, [FromQuery] string value)
        {
            try
            {
                await LogTraffic("GET - GetAllFiltered - Transaction - Async", "Request");

                var result = await _service.GetAllFilteredAsync(filterBy, value);

                await LogTraffic("GET - GetAllFiltered - Transaction - Async", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering transaction by {FilterBy} ", filterBy);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("ordered")]
        public async Task<IActionResult> GetAllOrdered([FromQuery] string orderBy, [FromQuery] string direction = "asc")
        {
            try
            {
                await LogTraffic("GET - GetAllOrdered - Transaction - Async", "Request");

                var result = await _service.GetAllOrderedAsync(orderBy, direction);

                await LogTraffic("GET - GetAllOrdered - Transaction - Async", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ordering transaction by {OrderBy} {Direction}", orderBy, direction);
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
