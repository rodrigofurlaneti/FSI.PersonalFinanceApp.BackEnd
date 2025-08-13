using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using FSI.PersonalFinanceApp.Api.Controllers.Base;

namespace FSI.PersonalFinanceApp.Api.Controllers;

[Route("api/expenses/sync")]
public class ExpenseControllerSync : BaseSyncController
{
    private readonly IExpenseAppService _service;

    public ExpenseControllerSync(IExpenseAppService service, ITrafficAppService traffic)
        : base(traffic) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ExpenseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public IActionResult GetAllSync()
    {
        Track("GET - Expense - Sync", "Request");
        var result = _service.GetAllSync();
        Track("GET - Expense - Sync", "Response");
        return OkOrNoContent(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public IActionResult GetById(long id)
    {
        Track("GET - GetById - Expense - Sync", "Request");
        var result = _service.GetByIdSync(id);
        Track("GET - GetById - Expense - Sync", "Response");
        return NotFoundOrOk(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public IActionResult Create([FromBody] ExpenseDto dto)
    {
        Track("POST - Create - Expense - Sync", "Request");
        _service.AddSync(dto);
        Track("POST - Create - Expense - Sync", "Response");
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public IActionResult Update(long id, [FromBody] ExpenseDto dto)
    {
        EnsureRouteMatchesBodyId(id, dto.Id);

        Track("PUT - Update - Expense - Sync", "Request");
        var existing = _service.GetByIdSync(id);
        if (existing is null) return NotFound();
        _service.UpdateSync(dto);
        Track("PUT - Update - Expense - Sync", "Response");
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public IActionResult Delete(long id)
    {
        Track("DELETE - Delete - Expense - Sync", "Request");
        var existing = _service.GetByIdSync(id);
        if (existing is null) return NotFound();
        _service.DeleteSync(existing);
        Track("DELETE - Delete - Expense - Sync", "Response");
        return NoContent();
    }

    [HttpGet("filtered")]
    [ProducesResponseType(typeof(IEnumerable<ExpenseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public IActionResult GetAllFiltered([FromQuery] string filterBy, [FromQuery] string value)
    {
        Require((nameof(filterBy), filterBy), (nameof(value), value));

        Track("GET - GetAllFiltered - Expense - Sync", "Request");
        var result = _service.GetAllFilteredSync(filterBy, value);
        Track("GET - GetAllFiltered - Expense - Sync", "Response");
        return OkOrNoContent(result);
    }

    [HttpGet("ordered")]
    [ProducesResponseType(typeof(IEnumerable<ExpenseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public IActionResult GetAllOrdered([FromQuery] string orderBy, [FromQuery] string direction = "asc")
    {
        // Validação simples
        var errors = new Dictionary<string, string[]>();
        if (string.IsNullOrWhiteSpace(orderBy))
            errors["orderBy"] = new[] { "O parâmetro 'orderBy' é obrigatório." };
        if (!string.Equals(direction, "asc", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(direction, "desc", StringComparison.OrdinalIgnoreCase))
            errors["direction"] = new[] { "O parâmetro 'direction' deve ser 'asc' ou 'desc'." };
        if (errors.Count > 0)
            throw new ValidationException("Parâmetros de ordenação inválidos.", errors);

        Track("GET - GetAllOrdered - Expense - Sync", "Request");
        var result = _service.GetAllOrderedSync(orderBy, direction);
        Track("GET - GetAllOrdered - Expense - Sync", "Response");
        return OkOrNoContent(result);
    }
}
