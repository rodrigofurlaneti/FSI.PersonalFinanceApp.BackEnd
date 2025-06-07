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

        public TransactionControllerSync(ITransactionAppService service)
        {
            _service = service;
        }

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
        public IActionResult Create([FromBody] TransactionDto dto)
        {
            _service.AddSync(dto);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:long}")]
        public IActionResult Update(long id, [FromBody] TransactionDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            _service.UpdateSync(dto);
            return NoContent();
        }
    }
}
