using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FSI.PersonalFinanceApp.Api.Controllers
{
    [ApiController]
    [Route("api/expenses/sync")]
    public class ExpenseControllerSync : ControllerBase
    {
        private readonly IExpenseAppService _service;
        private readonly ITrafficAppService _serviceTraffic;

        public ExpenseControllerSync(IExpenseAppService service, ITrafficAppService serviceTraffic)
        {
            _service = service;
            _serviceTraffic = serviceTraffic;
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
        public IActionResult Create([FromBody] ExpenseDto dto)
        {
            //Log the request traffic
            TrafficDto trafficDtoRequest = new TrafficDto("POST - Create - Expense - Sync", "Request", DateTime.Now);
            _serviceTraffic.AddSync(trafficDtoRequest);

            //Add the expense
            _service.AddSync(dto);

            //Log the response traffic
            TrafficDto trafficDtoResponse = new TrafficDto("POST - Create - Expense - Sync", "Response", DateTime.Now);
            _serviceTraffic.AddSync(trafficDtoResponse);

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:long}")]
        public IActionResult Update(long id, [FromBody] ExpenseDto dto)
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

        #region Additional Methods  

        //Name
        [HttpGet("GetAllOrderByNameAsc")]
        public IActionResult GetAllOrderByNameAsc()
        {
            var result = _service.GetAll_Orderby_Name_Asc_Sync();
            return Ok(result);
        }

        [HttpGet("GetAllOrderByNameDesc")]
        public IActionResult GetAllOrderByNameDesc()
        {
            var result = _service.GetAll_Orderby_Name_Desc_Sync();
            return Ok(result);
        }

        //Description
        [HttpGet("GetAllOrderByDescriptionAsc")]
        public IActionResult GetAllOrderByDescriptionAsc()
        {
            var result = _service.GetAll_Orderby_Description_Asc_Sync();
            return Ok(result);
        }

        [HttpGet("GetAllOrderByDescriptionDesc")]
        public IActionResult GetAllOrderByDescriptionDesc()
        {
            var result = _service.GetAll_Orderby_Description_Desc_Sync();
            return Ok(result);
        }

        //DueDate
        [HttpGet("GetAllOrderByDueDateAsc")]
        public IActionResult GetAllOrderByDueDateAsc()
        {
            var result = _service.GetAll_Orderby_DueDate_Asc_Sync();
            return Ok(result);
        }

        [HttpGet("GetAllOrderByDueDateDesc")]
        public IActionResult GetAllOrderByDueDateDesc()
        {
            var result = _service.GetAll_Orderby_DueDate_Desc_Sync();
            return Ok(result);
        }

        //PaidAt
        [HttpGet("GetAllOrderByPaidAtAsc")]
        public IActionResult GetAllOrderByPaidAtAsc()
        {
            var result = _service.GetAll_Orderby_PaidAt_Asc_Sync();
            return Ok(result);
        }

        [HttpGet("GetAllOrderByPaidAtDesc")]
        public IActionResult GetAllOrderByPaidAtDesc()
        {
            var result = _service.GetAll_Orderby_PaidAt_Desc_Sync();
            return Ok(result);
        }

        //Amount
        [HttpGet("GetAllOrderByAmountAsc")]
        public IActionResult GetAllOrderByAmountAsc()
        {
            var result = _service.GetAll_Orderby_Amount_Asc_Sync();
            return Ok(result);
        }

        [HttpGet("GetAllOrderByAmountDesc")]
        public IActionResult GetAllOrderByAmountDesc()
        {
            var result = _service.GetAll_Orderby_Amount_Desc_Sync();
            return Ok(result);
        }

        //ExpenseCategoryId
        [HttpGet("GetAllOrderByExpenseCategoryIdAsc")]
        public IActionResult GetAllOrderByExpenseCategoryIdAsc()
        {
            var result = _service.GetAll_Orderby_Amount_Asc_Sync();
            return Ok(result);
        }

        [HttpGet("GetAllOrderByExpenseCategoryIdDesc")]
        public IActionResult GetAllOrderByExpenseCategoryIdDesc()
        {
            var result = _service.GetAll_Orderby_ExpenseCategoryId_Desc_Sync();
            return Ok(result);
        }

        #endregion
    }

}
