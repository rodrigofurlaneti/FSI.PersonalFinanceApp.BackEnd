using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/expenses/async")]
    public class ExpenseControllerAsync : ControllerBase
    {
        private readonly IExpenseAppService _service;

        public ExpenseControllerAsync(IExpenseAppService service)
        {
            _service = service;
        }

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
        public async Task<IActionResult> Create([FromBody] ExpenseDto dto)
        {
            await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] ExpenseDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            await _service.UpdateAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var expenseDtoExisting = await _service.GetByIdAsync(id);
            if (expenseDtoExisting is null) return NotFound();
            await _service.DeleteAsync(expenseDtoExisting);
            return NoContent();
        }
    }
}
