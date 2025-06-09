using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Domain.Entities;
using System.Collections.Generic;

namespace FSI.PersonalFinanceApp.Application.Interfaces
{
    public interface IExpenseAppService : IBaseAppService<ExpenseDto>
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
