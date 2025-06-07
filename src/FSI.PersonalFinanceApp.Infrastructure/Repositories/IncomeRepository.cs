using Dapper;
using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FSI.PersonalFinanceApp.Infrastructure.Context;
using System.Data;

namespace FSI.PersonalFinanceApp.Infrastructure.Repositories
{
    public class IncomeRepository : BaseRepository, IIncomeRepository
    {
        public IncomeRepository(IDbContext context) : base(context) { }
        public async Task<IEnumerable<IncomeEntity>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<IncomeEntity>(
                "usp_Income_GetAll",
                commandType: CommandType.StoredProcedure
            );
        }

        public IEnumerable<IncomeEntity> GetAllSync()
        {
            using var connection = CreateConnection();
            return connection.Query<IncomeEntity>(
                "usp_Income_GetAll",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IncomeEntity?> GetByIdAsync(long id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<IncomeEntity>(
                "usp_Income_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public IncomeEntity? GetByIdSync(long id)
        {
            using var connection = CreateConnection();
            return connection.QueryFirstOrDefault<IncomeEntity>(
                "usp_Income_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task AddAsync(IncomeEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_Income_Add",
                new
                {
                    entity.Name,
                    entity.Amount,
                    entity.IncomeDate,
                    entity.Description,
                    entity.ReceivedAt,
                    entity.IncomeCategoryId,
                    entity.IsActive,
                    entity.CreatedAt,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void AddSync(IncomeEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute(
                "usp_Income_Add",
                new
                {
                    entity.Name,
                    entity.Amount,
                    entity.IncomeDate,
                    entity.Description,
                    entity.ReceivedAt,
                    entity.IncomeCategoryId,
                    entity.IsActive,
                    entity.CreatedAt,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateAsync(IncomeEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_Income_Update",
                new
                {
                    entity.Id,
                    entity.Name,
                    entity.Amount,
                    entity.IncomeDate,
                    entity.Description,
                    entity.ReceivedAt,
                    entity.IncomeCategoryId,
                    entity.IsActive,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void UpdateSync(IncomeEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute(
                "usp_Income_Update",
                new
                {
                    entity.Id,
                    entity.Name,
                    entity.Amount,
                    entity.IncomeDate,
                    entity.Description,
                    entity.ReceivedAt,
                    entity.IncomeCategoryId,
                    entity.IsActive,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeleteAsync(IncomeEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_Income_Delete",
                new
                {
                    entity.Id,
                    entity.Name,
                    entity.Amount,
                    entity.IncomeDate,
                    entity.Description,
                    entity.ReceivedAt,
                    entity.IncomeCategoryId,
                    entity.IsActive,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
