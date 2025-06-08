using FSI.PersonalFinanceApp.Domain.Entities;

namespace FSI.PersonalFinanceApp.Domain.Interfaces
{
    public interface IExpenseRepository : IBaseRepository<ExpenseEntity>
    {
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Description_Asc_Async();
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Description_Desc_Async();
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Name_Asc_Async();
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Name_Desc_Async();
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_DueDate_Asc_Async();
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_DueDate_Desc_Async();
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_PaidAt_Asc_Async();
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_PaidAt_Desc_Async();
    }
}
