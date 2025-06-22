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

        public async Task<long> AddAsync(UserEntity entity)
        {
            using var connection = CreateConnection();

            var id = await connection.ExecuteScalarAsync<long>("usp_User_Add",
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

            return id;
        }

        public long AddSync(UserEntity entity)
        {
            using var connection = CreateConnection();

            var id = connection.ExecuteScalar<long>("usp_User_Add",
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

            return id;
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

        public void DeleteSync(UserEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute("usp_User_Delete", new
            {
                entity.Id
            }, commandType: CommandType.StoredProcedure);
        }

        #region Methods for filtering user

        public async Task<IEnumerable<UserEntity>> GetAllFilteredAsync(string filterBy, string value)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filter by field");

            var procedureName = GetFilteredProcedureName(filterBy);

            var parameterName = _orderMap[filterBy];

            var parameters = new DynamicParameters();

            parameters.Add(parameterName, value);

            return await connection.QueryAsync<UserEntity>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<UserEntity> GetAllFilteredSync(string filterBy, string value)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filter by field");

            var procedureName = GetFilteredProcedureName(filterBy);

            var parameterName = _orderMap[filterBy];

            var parameters = new DynamicParameters();

            parameters.Add(parameterName, value);

            return connection.Query<UserEntity>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        #endregion

        public async Task<IEnumerable<UserEntity>> GetAllOrderedAsync(string orderBy, string direction)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);

            var procedureName = GetProcedureName(orderBy, direction);

            return await connection.QueryAsync<UserEntity>(procedureName, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<UserEntity> GetAllOrderedSync(string orderBy, string direction)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);

            var procedureName = GetProcedureName(orderBy, direction);

            return connection.Query<UserEntity>(procedureName, commandType: CommandType.StoredProcedure);
        }

        private string GetFilteredProcedureName(string filterBy)
        {
            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filterBy field");

            return $"usp_User_GetAll_FilterBy_{_orderMap[filterBy]}";
        }

        private string GetProcedureName(string orderBy, string direction)
        {
            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);
            return $"usp_User_GetAll_OrderBy_{_orderMap[orderBy]}_{(isDesc ? "Desc" : "Asc")}";
        }

        private static readonly Dictionary<string, string> _orderMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Name", "Name" }
        };
    }
}
