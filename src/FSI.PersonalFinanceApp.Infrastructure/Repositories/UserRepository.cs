using Dapper;
using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FSI.PersonalFinanceApp.Infrastructure.Context;
using System.Data;

namespace FSI.PersonalFinanceApp.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IDbContext context) : base(context) { }
        public async Task<IEnumerable<UserEntity>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<UserEntity>(
                "usp_User_GetAll",
                commandType: CommandType.StoredProcedure
            );
        }

        public IEnumerable<UserEntity> GetAllSync()
        {
            using var connection = CreateConnection();
            return connection.Query<UserEntity>(
                "usp_User_GetAll",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<UserEntity?> GetByIdAsync(long id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<UserEntity>(
                "usp_User_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public UserEntity? GetByIdSync(long id)
        {
            using var connection = CreateConnection();
            return connection.QueryFirstOrDefault<UserEntity>(
                "usp_User_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task AddAsync(UserEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_User_Add",
                new
                {
                    entity.Name,
                    entity.Email,
                    entity.PasswordHash,
                    entity.IsActive,
                    entity.CreatedAt,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void AddSync(UserEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute(
                "usp_User_Add",
                new
                {
                    entity.Name,
                    entity.Email,
                    entity.PasswordHash,
                    entity.IsActive,
                    entity.CreatedAt,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateAsync(UserEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_User_Update",
                new
                {
                    entity.Id,
                    entity.Name,
                    entity.Email,
                    entity.PasswordHash,
                    entity.IsActive,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void UpdateSync(UserEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute(
                "usp_User_Update",
                new
                {
                    entity.Id,
                    entity.Name,
                    entity.Email,
                    entity.PasswordHash,
                    entity.IsActive,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeleteAsync(UserEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_User_Delete",
                new
                {
                    entity.Id,
                    entity.Name,
                    entity.Email,
                    entity.PasswordHash,
                    entity.IsActive,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
