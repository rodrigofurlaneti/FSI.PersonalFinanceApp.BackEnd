using Dapper;
using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FSI.PersonalFinanceApp.Infrastructure.Context;
using System.Data;

namespace FSI.PersonalFinanceApp.Infrastructure.Repositories
{
    public class ExpenseCategoryRepository : BaseRepository, IExpenseCategoryRepository
    {
        public ExpenseCategoryRepository(IDbContext context) : base(context) { }
        public async Task<IEnumerable<ExpenseCategoryEntity>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ExpenseCategoryEntity>(
                "usp_ExpenseCategory_GetAll",
                commandType: CommandType.StoredProcedure
            );
        }

        public IEnumerable<ExpenseCategoryEntity> GetAllSync()
        {
            using var connection = CreateConnection();
            return connection.Query<ExpenseCategoryEntity>(
                "usp_ExpenseCategory_GetAll",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<ExpenseCategoryEntity?> GetByIdAsync(long id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ExpenseCategoryEntity>(
                "usp_ExpenseCategory_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public ExpenseCategoryEntity? GetByIdSync(long id)
        {
            using var connection = CreateConnection();
            return connection.QueryFirstOrDefault<ExpenseCategoryEntity>(
                "usp_ExpenseCategory_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task AddAsync(ExpenseCategoryEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_ExpenseCategory_Add",
                new
                {
                    entity.Name,
                    entity.IsActive,
                    entity.CreatedAt,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void AddSync(ExpenseCategoryEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute(
                "usp_ExpenseCategory_Add",
                new
                {
                    entity.Name,
                    entity.IsActive,
                    entity.CreatedAt,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateAsync(ExpenseCategoryEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_ExpenseCategory_Update",
                new
                {
                    entity.Id,
                    entity.Name,
                    entity.IsActive,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void UpdateSync(ExpenseCategoryEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute(
                "usp_ExpenseCategory_Update",
                new
                {
                    entity.Id,
                    entity.Name,
                    entity.IsActive,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeleteAsync(ExpenseCategoryEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_ExpenseCategory_Delete",
                new
                {
                    entity.Id,
                    entity.Name,
                    entity.IsActive,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteSync(ExpenseCategoryEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute("usp_ExpenseCategory_Delete", new
            {
                entity.Id
            }, commandType: CommandType.StoredProcedure);
        }
    }

}
