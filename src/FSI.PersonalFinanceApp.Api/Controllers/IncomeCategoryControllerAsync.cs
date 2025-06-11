using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/income-categories/async")]
    public class IncomeCategoryControllerAsync : ControllerBase
    {
        private readonly IIncomeCategoryAppService _service;

        public IncomeCategoryControllerAsync(IIncomeCategoryAppService service)
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
        public async Task<IActionResult> Create([FromBody] IncomeCategoryDto dto)
        {
            await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] IncomeCategoryDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            await _service.UpdateAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var incomeCategoryDtoExisting = await _service.GetByIdAsync(id);
            if (incomeCategoryDtoExisting is null) return NotFound();
            await _service.DeleteAsync(incomeCategoryDtoExisting);
            return NoContent();
        }

        #endregion

        #region Additional Methods
        // Add any additional methods specific to income categories here, if needed.
        #endregion
    }
}
