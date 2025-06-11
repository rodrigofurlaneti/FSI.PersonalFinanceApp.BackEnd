using FSI.PersonalFinanceApp.Application.Dtos;

namespace FSI.PersonalFinanceApp.Application.Interfaces
{
    public interface IExpenseAppService : IBaseAppService<ExpenseDto>
    {
        Task<IEnumerable<ExpenseDto>> GetAllOrderedAsync(string orderBy, string direction);
        IEnumerable<ExpenseDto> GetAllOrderedSync(string orderBy, string direction);
    }
}
