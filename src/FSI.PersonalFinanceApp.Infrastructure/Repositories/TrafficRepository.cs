using Dapper;
using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FSI.PersonalFinanceApp.Infrastructure.Context;
using System.Data;
using static Dapper.SqlMapper;

namespace FSI.PersonalFinanceApp.Infrastructure.Repositories
{
    public class TrafficRepository : BaseRepository, ITrafficRepository
    {
        public TrafficRepository(IDbContext context) : base(context) { }

        #region Methods based on ITrafficRepository interface

        public async Task<IEnumerable<TrafficEntity>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<TrafficEntity>("usp_Traffic_GetAll", commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<TrafficEntity> GetAllSync()
        {
            using var connection = CreateConnection();
            return connection.Query<TrafficEntity>("usp_Traffic_GetAll", commandType: CommandType.StoredProcedure);
        }

        public async Task<TrafficEntity?> GetByIdAsync(long id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<TrafficEntity>(
                "usp_Traffic_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public TrafficEntity? GetByIdSync(long id)
        {
            using var connection = CreateConnection();
            return connection.QueryFirstOrDefault<TrafficEntity>(
                "usp_Traffic_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<long> AddAsync(TrafficEntity entity)
        {
            using var connection = CreateConnection();

            var id = await connection.ExecuteScalarAsync<long>("usp_Traffic_Add", new
            {
                entity.Method,
                entity.Action,
                entity.BackEndCreatedAt, 
                entity.IsActive,
                entity.CreatedAt,
                entity.UpdatedAt
            }, commandType: CommandType.StoredProcedure);

            return id;
        }

        public long AddSync(TrafficEntity entity)
        {
            using var connection = CreateConnection();

            var id = connection.ExecuteScalar<long>("usp_Traffic_Add", new
            {
                entity.Method,
                entity.Action,
                entity.BackEndCreatedAt,
                entity.IsActive,
                entity.CreatedAt,
                entity.UpdatedAt
            }, commandType: CommandType.StoredProcedure);

            return id;
        }

        public async Task<bool> UpdateAsync(TrafficEntity entity)
        {
            using var connection = CreateConnection();

            var returnStoredProcedure = await connection.ExecuteScalarAsync<bool>(
                "usp_Traffic_Update", new
                {
                    entity.Id,
                    entity.Method,
                    entity.Action,
                    entity.IsActive,
                    entity.CreatedAt,
                    entity.UpdatedAt
                }, commandType: CommandType.StoredProcedure);

            return returnStoredProcedure;
        }

        public bool UpdateSync(TrafficEntity entity)
        {
            using var connection = CreateConnection();

            var returnStoredProcedure = connection.ExecuteScalar<bool>(
                "usp_Traffic_Update", new
                {
                    entity.Id,
                    entity.Method,
                    entity.Action,
                    entity.IsActive,
                    entity.CreatedAt,
                    entity.UpdatedAt
                }, commandType: CommandType.StoredProcedure);

            return returnStoredProcedure;
        }

        public async Task<bool> DeleteAsync(TrafficEntity entity)
        {
            using var connection = CreateConnection();

            var returnStoredProcedure = await connection.ExecuteScalarAsync<bool>(
                "usp_Traffic_Delete", new
                {
                    entity.Id
                }, commandType: CommandType.StoredProcedure);

            return returnStoredProcedure;
        }

        public bool DeleteSync(TrafficEntity entity)
        {
            using var connection = CreateConnection();

            var returnStoredProcedure = connection.ExecuteScalar<bool>(
                "usp_Traffic_Delete", new
                {
                    entity.Id
                }, commandType: CommandType.StoredProcedure);

            return returnStoredProcedure;
        }

        #endregion

        #region Methods for filtering traffic

        public async Task<IEnumerable<TrafficEntity>> GetAllFilteredAsync(string filterBy, string value)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filter by field");

            var procedureName = GetFilteredProcedureName(filterBy);

            var parameterName = _orderMap[filterBy];

            var parameters = new DynamicParameters();

            parameters.Add(parameterName, value);

            return await connection.QueryAsync<TrafficEntity>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<TrafficEntity> GetAllFilteredSync(string filterBy, string value)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filter by field");

            var procedureName = GetFilteredProcedureName(filterBy);

            var parameterName = _orderMap[filterBy];

            var parameters = new DynamicParameters();

            parameters.Add(parameterName, value);

            return connection.Query<TrafficEntity>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Methods for ordering traffic

        public async Task<IEnumerable<TrafficEntity>> GetAllOrderedAsync(string orderBy, string direction)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);

            var procedureName = GetProcedureName(orderBy, direction);

            return await connection.QueryAsync<TrafficEntity>(procedureName, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<TrafficEntity> GetAllOrderedSync(string orderBy, string direction)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);

            var procedureName = GetProcedureName(orderBy, direction);

            return connection.Query<TrafficEntity>(procedureName, commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Additional Methods Private

        private string GetFilteredProcedureName(string filterBy)
        {
            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filterBy field");

            return $"usp_Traffic_GetAll_FilterBy_{_orderMap[filterBy]}";
        }

        private string GetProcedureName(string orderBy, string direction)
        {
            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);
            return $"usp_Traffic_GetAll_OrderBy_{_orderMap[orderBy]}_{(isDesc ? "Desc" : "Asc")}";
        }

        private static readonly Dictionary<string, string> _orderMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Name", "Name" }
        };

        #endregion
    }
}