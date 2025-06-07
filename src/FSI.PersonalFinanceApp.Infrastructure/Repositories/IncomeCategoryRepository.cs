using Dapper;
using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FSI.PersonalFinanceApp.Infrastructure.Context;
using System.Data;

namespace FSI.PersonalFinanceApp.Infrastructure.Repositories
{
    public class IncomeCategoryRepository : BaseRepository, IIncomeCategoryRepository
    {
        public IncomeCategoryRepository(IDbContext context) : base(context) { }
        public async Task<IEnumerable<IncomeCategoryEntity>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<IncomeCategoryEntity>(
                "usp_IncomeCategory_GetAll",
                commandType: CommandType.StoredProcedure
            );
        }

        public IEnumerable<IncomeCategoryEntity> GetAllSync()
        {
            using var connection = CreateConnection();
            return connection.Query<IncomeCategoryEntity>(
                "usp_IncomeCategory_GetAll",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IncomeCategoryEntity?> GetByIdAsync(long id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<IncomeCategoryEntity>(
                "usp_IncomeCategory_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public IncomeCategoryEntity? GetByIdSync(long id)
        {
            using var connection = CreateConnection();
            return connection.QueryFirstOrDefault<IncomeCategoryEntity>(
                "usp_IncomeCategory_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task AddAsync(IncomeCategoryEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_IncomeCategory_Add",
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

        public void AddSync(IncomeCategoryEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute(
                "usp_IncomeCategory_Add",
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

        public async Task UpdateAsync(IncomeCategoryEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_IncomeCategory_Update",
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

        public void UpdateSync(IncomeCategoryEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute(
                "usp_IncomeCategory_Update",
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

        public async Task DeleteAsync(IncomeCategoryEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_IncomeCategory_Delete",
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
    }
}
