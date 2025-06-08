using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Domain.Entities;
using System.Collections.Generic;

namespace FSI.PersonalFinanceApp.Application.Interfaces
{
    public interface IExpenseAppService : IBaseAppService<ExpenseDto>
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
