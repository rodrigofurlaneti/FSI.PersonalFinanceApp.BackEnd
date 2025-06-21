using FSI.PersonalFinanceApp.Domain.Entities;

namespace FSI.PersonalFinanceApp.Domain.Interfaces
{
    public interface IExpenseRepository : IBaseRepository<ExpenseEntity>
    {
        Task<IEnumerable<ExpenseEntity>> GetAllOrderedAsync(string orderBy, string direction);
        IEnumerable<ExpenseEntity> GetAllOrderedSync(string orderBy, string direction);
    }
}
