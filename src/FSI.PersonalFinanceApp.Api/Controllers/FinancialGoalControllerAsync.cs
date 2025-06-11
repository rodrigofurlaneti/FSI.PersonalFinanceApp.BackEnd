using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/financial-goals/async")]
    public class FinancialGoalControllerAsync : ControllerBase
    {
        private readonly IFinancialGoalAppService _service;

        public FinancialGoalControllerAsync(IFinancialGoalAppService service)
        {
            _service = service;
        }

        #region CRUD Operations

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FinancialGoalDto dto)
        {
            await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] FinancialGoalDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            await _service.UpdateAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var financialGoalDtoExisting = await _service.GetByIdAsync(id);
            if (financialGoalDtoExisting is null) return NotFound();
            await _service.DeleteAsync(financialGoalDtoExisting);
            return NoContent();
        }

        #endregion

        #region Additional Methods
        // Add any additional methods specific to financial goals here, if needed.
        #endregion
    }
}
