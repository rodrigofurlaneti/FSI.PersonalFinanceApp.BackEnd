using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/users/async")]
    public class UserControllerAsync : ControllerBase
    {
        private readonly IUserAppService _service;

        public UserControllerAsync(IUserAppService service)
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
        public async Task<IActionResult> Create([FromBody] UserDto dto)
        {
            await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UserDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            await _service.UpdateAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var userDtoExisting = await _service.GetByIdAsync(id);
            if (userDtoExisting is null) return NotFound();
            await _service.DeleteAsync(userDtoExisting);
            return NoContent();
        }
    }
}
