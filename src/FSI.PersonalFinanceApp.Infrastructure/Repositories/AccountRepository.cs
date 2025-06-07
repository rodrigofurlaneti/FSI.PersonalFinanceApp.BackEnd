using Dapper;
using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FSI.PersonalFinanceApp.Infrastructure.Context;
using System.Data;

namespace FSI.PersonalFinanceApp.Infrastructure.Repositories
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        public AccountRepository(IDbContext context) : base(context) { }
        public async Task<IEnumerable<AccountEntity>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<AccountEntity>(
                "usp_Account_GetAll",
                commandType: CommandType.StoredProcedure
            );
        }

        public IEnumerable<AccountEntity> GetAllSync()
        {
            using var connection = CreateConnection();
            return connection.Query<AccountEntity>(
                "usp_Account_GetAll",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<AccountEntity?> GetByIdAsync(long id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AccountEntity>(
                "usp_Account_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public AccountEntity? GetByIdSync(long id)
        {
            using var connection = CreateConnection();
            return connection.QueryFirstOrDefault<AccountEntity>(
                "usp_Account_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task AddAsync(AccountEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_Account_Add",
                new
                {
                    entity.Name,
                    entity.Description,
                    entity.Balance,
                    entity.IsActive,
                    entity.CreatedAt,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void AddSync(AccountEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute(
                "usp_Account_Add",
                new
                {
                    entity.Name,
                    entity.Description,
                    entity.Balance,
                    entity.IsActive,
                    entity.CreatedAt,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateAsync(AccountEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_Account_Update",
                new
                {
                    entity.Id,
                    entity.Name,
                    entity.Description,
                    entity.Balance,
                    entity.IsActive,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void UpdateSync(AccountEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute(
                "usp_Account_Update",
                new
                {
                    entity.Id,
                    entity.Name,
                    entity.Description,
                    entity.Balance,
                    entity.IsActive,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeleteAsync(AccountEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_Account_Delete",
                new
                {
                    entity.Id,
                    entity.Name,
                    entity.Description,
                    entity.Balance,
                    entity.IsActive,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
