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

        public async Task<long> AddAsync(IncomeEntity entity)
        {
            using var connection = CreateConnection();

            var id = await connection.ExecuteScalarAsync<long>(
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

            return id;
        }

        public long AddSync(IncomeEntity entity)
        {
            using var connection = CreateConnection();

            var id = connection.ExecuteScalar<long>("usp_Income_Add",
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

            return id;
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

        public void DeleteSync(IncomeEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute("usp_Income_Delete", new
            {
                entity.Id
            }, commandType: CommandType.StoredProcedure);
        }

        #region Methods for filtering expenses

        public async Task<IEnumerable<IncomeEntity>> GetAllFilteredAsync(string filterBy, string value)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filter by field");

            var procedureName = GetFilteredProcedureName(filterBy);

            var parameterName = _orderMap[filterBy];

            var parameters = new DynamicParameters();

            parameters.Add(parameterName, value);

            return await connection.QueryAsync<IncomeEntity>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<IncomeEntity> GetAllFilteredSync(string filterBy, string value)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filter by field");

            var procedureName = GetFilteredProcedureName(filterBy);

            var parameterName = _orderMap[filterBy];

            var parameters = new DynamicParameters();

            parameters.Add(parameterName, value);

            return connection.Query<IncomeEntity>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Methods for ordering expenses

        public async Task<IEnumerable<IncomeEntity>> GetAllOrderedAsync(string orderBy, string direction)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);

            var procedureName = GetProcedureName(orderBy, direction);

            return await connection.QueryAsync<IncomeEntity>(procedureName, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<IncomeEntity> GetAllOrderedSync(string orderBy, string direction)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);

            var procedureName = GetProcedureName(orderBy, direction);

            return connection.Query<IncomeEntity>(procedureName, commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Additional Methods Private

        private string GetFilteredProcedureName(string filterBy)
        {
            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filterBy field");

            return $"usp_Income_GetAll_FilterBy_{_orderMap[filterBy]}";
        }

        private string GetProcedureName(string orderBy, string direction)
        {
            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);
            return $"usp_Income_GetAll_OrderBy_{_orderMap[orderBy]}_{(isDesc ? "Desc" : "Asc")}";
        }

        private static readonly Dictionary<string, string> _orderMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Name", "Name" }
        };

        #endregion
    }
}
