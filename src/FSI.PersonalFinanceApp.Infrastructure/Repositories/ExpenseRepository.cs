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
    }
}
