using FSI.PersonalFinanceApp.Domain.Entities;

namespace FSI.PersonalFinanceApp.Domain.Interfaces
{
    public interface IExpenseRepository : IBaseRepository<ExpenseEntity>
    {
        //Description Async
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Description_Asc_Async();
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Description_Desc_Async();

        //Description Sync
        IEnumerable<ExpenseEntity> GetAll_Orderby_Description_Asc_Sync();
        IEnumerable<ExpenseEntity> GetAll_Orderby_Description_Desc_Sync();

        //Name Async
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Name_Asc_Async();
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Name_Desc_Async();

        //Name Sync
        IEnumerable<ExpenseEntity> GetAll_Orderby_Name_Asc_Sync();
        IEnumerable<ExpenseEntity> GetAll_Orderby_Name_Desc_Sync();

        //DueDate Async
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_DueDate_Asc_Async();
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_DueDate_Desc_Async();

        //DueDate Sync
        IEnumerable<ExpenseEntity> GetAll_Orderby_DueDate_Asc_Sync();
        IEnumerable<ExpenseEntity> GetAll_Orderby_DueDate_Desc_Sync();

        //PaidAt Async
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_PaidAt_Asc_Async();
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_PaidAt_Desc_Async();

        //PaidAt Sync
        IEnumerable<ExpenseEntity> GetAll_Orderby_PaidAt_Asc_Sync();
        IEnumerable<ExpenseEntity> GetAll_Orderby_PaidAt_Desc_Sync();

        //Amount Async
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Amount_Asc_Async();
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Amount_Desc_Async();

        //Amount Sync
        IEnumerable<ExpenseEntity> GetAll_Orderby_Amount_Asc_Sync();
        IEnumerable<ExpenseEntity> GetAll_Orderby_Amount_Desc_Sync();

        //ExpenseCategoryId Async
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_ExpenseCategoryId_Asc_Async();
        Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_ExpenseCategoryId_Desc_Async();

        //ExpenseCategoryId Sync
        IEnumerable<ExpenseEntity> GetAll_Orderby_ExpenseCategoryId_Asc_Sync();
        IEnumerable<ExpenseEntity> GetAll_Orderby_ExpenseCategoryId_Desc_Sync();

    }
}
