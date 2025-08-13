using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FSI.PersonalFinanceApp.Api.Controllers.Base;

[ApiController]
public abstract class BaseSyncController : ControllerBase
{
    private readonly ITrafficAppService _traffic;

    protected BaseSyncController(ITrafficAppService traffic)
        => _traffic = traffic;

    // ---- LOG/TRACK ---------------------------------------------------------
    protected void Track(string method, string action)
        => _traffic.AddSync(new TrafficDto(method, action, DateTime.UtcNow));

    // ---- RESPONSES CONVENIENCE --------------------------------------------
    protected IActionResult OkOrNoContent<T>(IEnumerable<T>? items)
        => (items is null || !items.Any()) ? NoContent() : Ok(items);

    protected IActionResult NotFoundOrOk<T>(T? item)
        => item is null ? NotFound() : Ok(item);

    // ---- VALIDATION HELPERS ------------------------------------------------
    protected void EnsureRouteMatchesBodyId(long routeId, long bodyId)
    {
        if (routeId != bodyId)
            throw new ValidationException("ID do caminho difere do ID do corpo.",
                new Dictionary<string, string[]>
                {
                    ["id"] = new[] { $"Route id = {routeId}, body id = {bodyId}" }
                });
    }

    protected void Require(params (string Name, string? Value)[] pairs)
    {
        var errors = new Dictionary<string, string[]>();
        foreach (var (name, value) in pairs)
        {
            if (string.IsNullOrWhiteSpace(value))
                errors[name] = new[] { $"O parâmetro '{name}' é obrigatório." };
        }
        if (errors.Count > 0)
            throw new ValidationException("Parâmetros inválidos.", errors);
    }
}
