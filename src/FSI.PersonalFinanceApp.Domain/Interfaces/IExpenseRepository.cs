using FSI.PersonalFinanceApp.Domain.Entities;

namespace FSI.PersonalFinanceApp.Domain.Interfaces
{
    public interface IExpenseRepository : IBaseRepository<ExpenseEntity>
    {
        //Description
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Description_Asc_Async();
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Description_Desc_Async();

        //Name
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Name_Asc_Async();
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Name_Desc_Async();

        //DueDate
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_DueDate_Asc_Async();
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_DueDate_Desc_Async();

        //PaidAt
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_PaidAt_Asc_Async();
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_PaidAt_Desc_Async();

        //Amount
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Amount_Asc_Async();
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Amount_Desc_Async();

        //ExpenseCategoryId
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_ExpenseCategoryId_Asc_Async();
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_ExpenseCategoryId_Desc_Async();
    }
}
