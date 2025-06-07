using Dapper;
using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FSI.PersonalFinanceApp.Infrastructure.Context;
using System.Data;

namespace FSI.PersonalFinanceApp.Infrastructure.Repositories
{
    public class FinancialGoalRepository : BaseRepository, IFinancialGoalRepository
    {
        public FinancialGoalRepository(IDbContext context) : base(context) { }
        public async Task<IEnumerable<FinancialGoalEntity>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<FinancialGoalEntity>(
                "usp_FinancialGoal_GetAll",
                commandType: CommandType.StoredProcedure
            );
        }

        public IEnumerable<FinancialGoalEntity> GetAllSync()
        {
            using var connection = CreateConnection();
            return connection.Query<FinancialGoalEntity>(
                "usp_FinancialGoal_GetAll",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<FinancialGoalEntity?> GetByIdAsync(long id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<FinancialGoalEntity>(
                "usp_FinancialGoal_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public FinancialGoalEntity? GetByIdSync(long id)
        {
            using var connection = CreateConnection();
            return connection.QueryFirstOrDefault<FinancialGoalEntity>(
                "usp_FinancialGoal_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task AddAsync(FinancialGoalEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_FinancialGoal_Add",
                new
                {
                    entity.Name,
                    entity.TargetAmount,
                    entity.CurrentAmount,
                    entity.DueDate,
                    entity.Description,
                    entity.IsCompleted,
                    entity.IsActive,
                    entity.CreatedAt,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void AddSync(FinancialGoalEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute(
                "usp_FinancialGoal_Add",
                new
                {
                    entity.Name,
                    entity.TargetAmount,
                    entity.CurrentAmount,
                    entity.DueDate,
                    entity.Description,
                    entity.IsCompleted,
                    entity.IsActive,
                    entity.CreatedAt,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateAsync(FinancialGoalEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_FinancialGoal_Update",
                new
                {
                    entity.Id,
                    entity.Name,
                    entity.TargetAmount,
                    entity.CurrentAmount,
                    entity.DueDate,
                    entity.Description,
                    entity.IsCompleted,
                    entity.IsActive,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void UpdateSync(FinancialGoalEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute(
                "usp_FinancialGoal_Update",
                new
                {
                    entity.Id,
                    entity.Name,
                    entity.TargetAmount,
                    entity.CurrentAmount,
                    entity.DueDate,
                    entity.Description,
                    entity.IsCompleted,
                    entity.IsActive,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeleteAsync(FinancialGoalEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_FinancialGoal_Delete",
                new
                {
                    entity.Id,
                    entity.Name,
                    entity.TargetAmount,
                    entity.CurrentAmount,
                    entity.DueDate,
                    entity.Description,
                    entity.IsCompleted,
                    entity.IsActive,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
