using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/accounts/sync")]
    public class AccountControllerSync : ControllerBase
    {
        private readonly IAccountAppService _service;
        private readonly ITrafficAppService _serviceTraffic;
        private readonly ILogger<AccountControllerSync> _logger;

        public AccountControllerSync(IAccountAppService service, ITrafficAppService serviceTraffic, ILogger<AccountControllerSync> logger)
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
                LogTraffic("GET - GetAll - Account - Sync", "Request");

                var result = _service.GetAllSync();

                LogTraffic("GET - GetAll - Account - Sync", "Response");

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting account");
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("{id:long}")]
        public IActionResult GetById(long id)
        {
            try
            {
                LogTraffic("GET - GetById - Account - Sync", "Request");

                var result = _service.GetByIdSync(id);

                LogTraffic("GET - GetById - Account - Sync", "Response");

                if (result is null)
                {
                    _logger.LogWarning("Account with id {AccountId} not found", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error retrieving account with id {AccountId}", id);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] AccountDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for account creation: {@AccountDto}", dto);
                    return BadRequest(ModelState);
                }

                LogTraffic("POST - Create - Account - Sync", "Request");

                _service.AddSync(dto);

                LogTraffic("POST - Create - Account - Sync", "Response");

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
        public IActionResult Update(long id, [FromBody] AccountDto dto)
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

                LogTraffic("PUT - Update - Account - Sync", "Request");

                var existingAccount = _service.GetByIdSync(id);
                if (existingAccount is null)
                {
                    _logger.LogWarning("Account with id {AccountId} not found for update", id);
                    return NotFound();
                }

                _service.UpdateSync(dto);

                LogTraffic("PUT - Update - Account - Sync", "Response");

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
        public IActionResult Delete(long id)
        {
            try
            {
                LogTraffic("DELETE - Delete - Account - Sync", "Request");

                var existingAccount = _service.GetByIdSync(id);
                if (existingAccount is null)
                {
                    _logger.LogWarning("Account with id {AccountId} not found for deletion", id);
                    return NotFound();
                }

                _service.DeleteSync(existingAccount);

                LogTraffic("DELETE - Delete - Account - Sync", "Response");

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
        public IActionResult GetAllFiltered([FromQuery] string filterBy, [FromQuery] string value)
        {
            try
            {
                LogTraffic("GET - GetAllFiltered - Account - Sync", "Request");

                var result = _service.GetAllFilteredSync(filterBy, value);

                LogTraffic("GET - GetAllFiltered - Account - Sync", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering account by {FilterBy} ", filterBy);
                return StatusCode(500, "Error processing request");
            }
        }

        [HttpGet("ordered")]
        public IActionResult GetAllOrdered([FromQuery] string orderBy, [FromQuery] string direction = "asc")
        {
            try
            {
                LogTraffic("GET - GetAllOrdered - Account - Sync", "Request");

                var result = _service.GetAllOrderedSync(orderBy, direction);

                LogTraffic("GET - GetAllOrdered - Account - Sync", "Response");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ordering account by {OrderBy} {Direction}", orderBy, direction);
                return StatusCode(500, "Error processing request");
            }
        }

        #endregion

        #region Methods Additional

        #endregion

        #region Additional Methods Private 

        private void LogTraffic(string method, string action)
        {
            var dto = new TrafficDto(method, action, DateTime.Now);
            _serviceTraffic.AddSync(dto);
        }

        #endregion
    }
}
