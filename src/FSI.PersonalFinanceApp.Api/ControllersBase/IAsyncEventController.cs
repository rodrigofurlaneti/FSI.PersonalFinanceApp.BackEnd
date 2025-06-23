using Microsoft.AspNetCore.Mvc;

namespace FSI.PersonalFinanceApp.Api.ControllersBase
{
    public interface IAsyncEventController<TDto> where TDto : class, new()
    {
        // Mensageria/Event Driven
        Task<IActionResult> MessageCreateAsync(TDto dto);
        Task<IActionResult> MessageUpdateAsync(long id, TDto dto);
        Task<IActionResult> MessageDeleteAsync(long id);
        Task<IActionResult> MessageGetByIdAsync(long id);
        Task<IActionResult> MessageGetAllAsync();
        Task<IActionResult> GetResultAsync(long id);
    }
}
