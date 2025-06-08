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

        #endregion

        #region Additional Methods  

        [HttpGet("GetAllOrderByNameAsc")]
        public async Task<IActionResult> GetAllOrderByNameAsc()
        {
            var result = await _service.GetAll_Orderby_Name_Asc_Async();
            return Ok(result);
        }
        
        [HttpGet("GetAllOrderByNameDesc")]
        public async Task<IActionResult> GetAllOrderByNameDesc()
        {
            var result = await _service.GetAll_Orderby_Name_Desc_Async();
            return Ok(result);
        }

        [HttpGet("GetAllOrderByDescriptionAsc")]
        public async Task<IActionResult> GetAllOrderByDescriptionAsc()
        {
            var result = await _service.GetAll_Orderby_Description_Asc_Async();
            return Ok(result);
        }

        [HttpGet("GetAllOrderByDescriptionDesc")]
        public async Task<IActionResult> GetAllOrderByDescriptionDesc()
        {
            var result = await _service.GetAll_Orderby_Description_Desc_Async();
            return Ok(result);
        }

        [HttpGet("GetAllOrderByDueDateAsc")]
        public async Task<IActionResult> GetAllOrderByDueDateAsc()
        {
            var result = await _service.GetAll_Orderby_DueDate_Asc_Async();
            return Ok(result);
        }

        [HttpGet("GetAllOrderByDueDateDesc")]
        public async Task<IActionResult> GetAllOrderByDueDateDesc()
        {
            var result = await _service.GetAll_Orderby_DueDate_Desc_Async();
            return Ok(result);
        }

        [HttpGet("GetAllOrderByPaidAtAsc")]
        public async Task<IActionResult> GetAllOrderByPaidAtAsc()
        {
            var result = await _service.GetAll_Orderby_PaidAt_Asc_Async();
            return Ok(result);
        }

        [HttpGet("GetAllOrderByPaidAtDesc")]
        public async Task<IActionResult> GetAllOrderByPaidAtDesc()
        {
            var result = await _service.GetAll_Orderby_PaidAt_Desc_Async();
            return Ok(result);
        }

        #endregion
    }
}
