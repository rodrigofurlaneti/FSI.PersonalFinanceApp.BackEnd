using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/transactions/sync")]
    public class TransactionControllerSync : ControllerBase
    {
        private readonly ITransactionAppService _service;
        private readonly ITrafficAppService _serviceTraffic;
        private readonly ILogger<TransactionControllerSync> _logger;

        public TransactionControllerSync(ITransactionAppService service, ITrafficAppService serviceTraffic, ILogger<TransactionControllerSync> logger)
        {
            _service = service;
            _serviceTraffic = serviceTraffic;
            _logger = logger;
        }

        #region CRUD Operations

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                LogTraffic("GET - GetAll - Transaction - Sync", "Request");

                var result = _service.GetAllSync();

                LogTraffic("GET - GetAll - Transaction - Sync", "Response");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting transaction");
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("{id:long}")]
        public IActionResult GetById(long id)
        {
            try
            {
                LogTraffic("GET - GetById - Transaction - Sync", "Request");

                var result = _service.GetByIdSync(id);

                LogTraffic("GET - GetById - Transaction - Sync", "Response");

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
        public IActionResult Create([FromBody] TransactionDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for transaction creation: {@TransactionDto}", dto);
                    return BadRequest(ModelState);
                }

                LogTraffic("POST - Create - Transaction - Sync", "Request");

                _service.AddSync(dto);

                LogTraffic("POST - Create - Transaction - Sync", "Response");

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
        public IActionResult Update(long id, [FromBody] TransactionDto dto)
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

                LogTraffic("PUT - Update - Transaction - Sync", "Request");

                var existingTransaction = _service.GetByIdSync(id);
                if (existingTransaction is null)
                {
                    _logger.LogWarning("Transaction with id {TransactionId} not found for update", id);
                    return NotFound();
                }

                _service.UpdateSync(dto);

                LogTraffic("PUT - Update - Transaction - Sync", "Response");

                _logger.LogInformation("Transaction with id {TransactionId} updated successfully", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating income with id {TransactionId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            try
            {
                LogTraffic("DELETE - Delete - Transaction - Sync", "Request");

                var existingTransaction = _service.GetByIdSync(id);
                if (existingTransaction is null)
                {
                    _logger.LogWarning("Transaction with id {TransactionId} not found for deletion", id);
                    return NotFound();
                }

                _service.DeleteSync(existingTransaction);

                LogTraffic("DELETE - Delete - Transaction - Sync", "Response");

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
        public IActionResult GetAllFiltered([FromQuery] string filterBy, [FromQuery] string value)
        {
            try
            {
                LogTraffic("GET - GetAllFiltered - Transaction - Sync", "Request");

                var result = _service.GetAllFilteredSync(filterBy, value);

                LogTraffic("GET - GetAllFiltered - Transaction - Sync", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering transaction by {FilterBy} ", filterBy);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("ordered")]
        public IActionResult GetAllOrdered([FromQuery] string orderBy, [FromQuery] string direction = "asc")
        {
            try
            {
                LogTraffic("GET - GetAllOrdered - Transaction - Sync", "Request");

                var result = _service.GetAllOrderedSync(orderBy, direction);

                LogTraffic("GET - GetAllOrdered - Transaction - Sync", "Response");

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
            _serviceTraffic.AddSync(dto);
        }

        #endregion
    }
}
