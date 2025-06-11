using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/expense-categories/sync")]
    public class ExpenseCategoryControllerSync : ControllerBase
    {
        private readonly IExpenseCategoryAppService _service;

        public ExpenseCategoryControllerSync(IExpenseCategoryAppService service)
        {
            _service = service;
        }

        #region CRUD Operations

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _service.GetAllSync();
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public IActionResult GetById(long id)
        {
            var result = _service.GetByIdSync(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ExpenseCategoryDto dto)
        {
            _service.AddSync(dto);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:long}")]
        public IActionResult Update(long id, [FromBody] ExpenseCategoryDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            _service.UpdateSync(dto);
            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            var expenseDtoExisting = _service.GetByIdSync(id);
            if (expenseDtoExisting is null) return NotFound();
            _service.DeleteSync(expenseDtoExisting);
            return NoContent();
        }

        #endregion
    }
}
