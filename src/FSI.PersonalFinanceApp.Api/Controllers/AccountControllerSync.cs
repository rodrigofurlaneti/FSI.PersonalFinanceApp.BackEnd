using FSI.PersonalFinanceApp.Api.Controllers.Base;
using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FSI.PersonalFinanceApp.Api.Controllers;

[ApiController]
[Route("api/accounts/sync")]
public class AccountControllerSync : BaseSyncController
{
    private readonly IAccountAppService _service;

    public AccountControllerSync(IAccountAppService service, ITrafficAppService traffic)
        : base(traffic)
    {
        _service = service;
    }

    #region CRUD Operations

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AccountDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public IActionResult GetAll()
    {
        Track("GET - GetAll - Account - Sync", "Request");
        var result = _service.GetAllSync();
        Track("GET - GetAll - Account - Sync", "Response");
        return OkOrNoContent(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public IActionResult GetById(long id)
    {
        Track("GET - GetById - Account - Sync", "Request");
        var result = _service.GetByIdSync(id);
        Track("GET - GetById - Account - Sync", "Response");
        return NotFoundOrOk(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public IActionResult Create([FromBody] AccountDto dto)
    {
        Track("POST - Create - Account - Sync", "Request");
        _service.AddSync(dto);
        Track("POST - Create - Account - Sync", "Response");
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public IActionResult Update(long id, [FromBody] AccountDto dto)
    {
        EnsureRouteMatchesBodyId(id, dto.Id);

        Track("PUT - Update - Account - Sync", "Request");
        var existing = _service.GetByIdSync(id);
        if (existing is null) return NotFound(); // ou: throw new NotFoundException(...)
        _service.UpdateSync(dto);
        Track("PUT - Update - Account - Sync", "Response");
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public IActionResult Delete(long id)
    {
        Track("DELETE - Delete - Account - Sync", "Request");
        var existing = _service.GetByIdSync(id);
        if (existing is null) return NotFound(); // ou: throw new NotFoundException(...)
        _service.DeleteSync(existing);
        Track("DELETE - Delete - Account - Sync", "Response");
        return NoContent();
    }

    [HttpGet("filtered")]
    [ProducesResponseType(typeof(IEnumerable<AccountDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public IActionResult GetAllFiltered([FromQuery] string filterBy, [FromQuery] string value)
    {
        Require((nameof(filterBy), filterBy), (nameof(value), value));

        Track("GET - GetAllFiltered - Account - Sync", "Request");
        var result = _service.GetAllFilteredSync(filterBy, value);
        Track("GET - GetAllFiltered - Account - Sync", "Response");
        return OkOrNoContent(result);
    }

    [HttpGet("ordered")]
    [ProducesResponseType(typeof(IEnumerable<AccountDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public IActionResult GetAllOrdered([FromQuery] string orderBy, [FromQuery] string direction = "asc")
    {
        var errors = new Dictionary<string, string[]>();
        if (string.IsNullOrWhiteSpace(orderBy))
            errors[nameof(orderBy)] = new[] { "O parâmetro 'orderBy' é obrigatório." };
        if (!string.Equals(direction, "asc", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(direction, "desc", StringComparison.OrdinalIgnoreCase))
            errors[nameof(direction)] = new[] { "O parâmetro 'direction' deve ser 'asc' ou 'desc'." };
        if (errors.Count > 0)
            throw new ValidationException("Parâmetros de ordenação inválidos.", errors);

        Track("GET - GetAllOrdered - Account - Sync", "Request");
        var result = _service.GetAllOrderedSync(orderBy, direction);
        Track("GET - GetAllOrdered - Account - Sync", "Response");
        return OkOrNoContent(result);
    }

    #endregion
}
