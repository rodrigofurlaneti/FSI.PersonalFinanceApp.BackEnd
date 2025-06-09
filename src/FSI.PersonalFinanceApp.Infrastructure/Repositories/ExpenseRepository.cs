using Dapper;
using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FSI.PersonalFinanceApp.Infrastructure.Context;
using System.Data;

namespace FSI.PersonalFinanceApp.Infrastructure.Repositories
{
    public class ExpenseRepository : BaseRepository, IExpenseRepository
    {
        public ExpenseRepository(IDbContext context) : base(context) { }

        #region Methods based on IExpenseRepository interface

        public async Task<IEnumerable<ExpenseEntity>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ExpenseEntity>("usp_Expense_GetAll", commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<ExpenseEntity> GetAllSync()
        {
            using var connection = CreateConnection();
            return connection.Query<ExpenseEntity>("usp_Expense_GetAll", commandType: CommandType.StoredProcedure);
        }

        public async Task<ExpenseEntity?> GetByIdAsync(long id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ExpenseEntity>(
                "usp_Expense_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public ExpenseEntity? GetByIdSync(long id)
        {
            using var connection = CreateConnection();
            return connection.QueryFirstOrDefault<ExpenseEntity>(
                "usp_Expense_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task AddAsync(ExpenseEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("usp_Expense_Add", new
            {
                entity.Name,
                entity.Amount,
                entity.DueDate,
                entity.Description,
                entity.PaidAt,
                entity.ExpenseCategoryId,
                entity.IsActive,
                entity.CreatedAt,
                entity.UpdatedAt
            }, commandType: CommandType.StoredProcedure);
        }

        public void AddSync(ExpenseEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute("usp_Expense_Add", new
            {
                entity.Name,
                entity.Amount,
                entity.DueDate,
                entity.Description,
                entity.PaidAt,
                entity.ExpenseCategoryId,
                entity.IsActive,
                entity.CreatedAt,
                entity.UpdatedAt
            }, commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(ExpenseEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("usp_Expense_Update", new
            {
                entity.Id,
                entity.Name,
                entity.Amount,
                entity.DueDate,
                entity.Description,
                entity.PaidAt,
                entity.ExpenseCategoryId,
                entity.IsActive,
                entity.UpdatedAt
            }, commandType: CommandType.StoredProcedure);
        }

        public void UpdateSync(ExpenseEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute("usp_Expense_Update", new
            {
                entity.Id,
                entity.Name,
                entity.Amount,
                entity.DueDate,
                entity.Description,
                entity.PaidAt,
                entity.ExpenseCategoryId,
                entity.IsActive,
                entity.UpdatedAt
            }, commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteAsync(ExpenseEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("usp_Expense_Delete", new
            {
                entity.Id
            }, commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Methods for ordering expenses

        //Description
        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Description_Asc_Async()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ExpenseEntity>("usp_Expense_GetAll_OrderBy_Description_Asc", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Description_Desc_Async()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ExpenseEntity>("usp_Expense_GetAll_OrderBy_Description_Desc", commandType: CommandType.StoredProcedure);
        }

        //Name
        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Name_Asc_Async()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ExpenseEntity>("usp_Expense_GetAll_OrderBy_Name_Asc", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Name_Desc_Async()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ExpenseEntity>("usp_Expense_GetAll_OrderBy_Name_Desc", commandType: CommandType.StoredProcedure);
        }

        //DueDate
        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_DueDate_Asc_Async()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ExpenseEntity>("usp_Expense_GetAll_OrderBy_DueDate_Asc", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_DueDate_Desc_Async()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ExpenseEntity>("usp_Expense_GetAll_OrderBy_DueDate_Desc", commandType: CommandType.StoredProcedure);
        }

        //PaidAt
        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_PaidAt_Asc_Async()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ExpenseEntity>("usp_Expense_GetAll_OrderBy_PaidAt_Asc", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_PaidAt_Desc_Async()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ExpenseEntity>("usp_Expense_GetAll_OrderBy_PaidAt_Desc", commandType: CommandType.StoredProcedure);
        }

        //Amount
        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Amount_Asc_Async()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ExpenseEntity>("usp_Expense_GetAll_OrderBy_Amount_Asc", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Amount_Desc_Async()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ExpenseEntity>("usp_Expense_GetAll_OrderBy_Amount_Desc", commandType: CommandType.StoredProcedure);
        }

        //ExpenseCategoryId
        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_ExpenseCategoryId_Asc_Async()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ExpenseEntity>("usp_Expense_GetAll_OrderBy_ExpenseCategoryId_Asc", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_ExpenseCategoryId_Desc_Async()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ExpenseEntity>("usp_Expense_GetAll_OrderBy_ExpenseCategoryId_Desc", commandType: CommandType.StoredProcedure);
        }

        #endregion
    }
}
