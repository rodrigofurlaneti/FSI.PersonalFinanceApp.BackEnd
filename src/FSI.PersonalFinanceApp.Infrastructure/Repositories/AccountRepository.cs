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

        public void DeleteSync(AccountEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute("usp_Account_Delete", new
            {
                entity.Id
            }, commandType: CommandType.StoredProcedure);
        }

        #region Methods for filtering account

        public async Task<IEnumerable<AccountEntity>> GetAllFilteredAsync(string filterBy, string value)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filter by field");

            var procedureName = GetFilteredProcedureName(filterBy);

            var parameterName = _orderMap[filterBy];

            var parameters = new DynamicParameters();

            parameters.Add(parameterName, value);

            return await connection.QueryAsync<AccountEntity>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<AccountEntity> GetAllFilteredSync(string filterBy, string value)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filter by field");

            var procedureName = GetFilteredProcedureName(filterBy);

            var parameterName = _orderMap[filterBy];

            var parameters = new DynamicParameters();

            parameters.Add(parameterName, value);

            return connection.Query<AccountEntity>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Methods for ordering account

        public async Task<IEnumerable<AccountEntity>> GetAllOrderedAsync(string orderBy, string direction)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);

            var procedureName = GetProcedureName(orderBy, direction);

            return await connection.QueryAsync<AccountEntity>(procedureName, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<AccountEntity> GetAllOrderedSync(string orderBy, string direction)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);

            var procedureName = GetProcedureName(orderBy, direction);

            return connection.Query<AccountEntity>(procedureName, commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Additional Methods Private

        private string GetFilteredProcedureName(string filterBy)
        {
            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filterBy field");

            return $"usp_Account_GetAll_FilterBy_{_orderMap[filterBy]}";
        }

        private string GetProcedureName(string orderBy, string direction)
        {
            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);
            return $"usp_Account_GetAll_OrderBy_{_orderMap[orderBy]}_{(isDesc ? "Desc" : "Asc")}";
        }

        private static readonly Dictionary<string, string> _orderMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Name", "Name" }
        };

        #endregion
    }
}
