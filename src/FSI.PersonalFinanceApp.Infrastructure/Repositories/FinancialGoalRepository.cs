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

        public async Task<long> AddAsync(FinancialGoalEntity entity)
        {
            using var connection = CreateConnection();

            var id = await connection.ExecuteScalarAsync<long>("usp_FinancialGoal_Add",
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

            return id;
        }

        public long AddSync(FinancialGoalEntity entity)
        {
            using var connection = CreateConnection();

            var id = connection.ExecuteScalar<long>("usp_FinancialGoal_Add",
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

            return id;
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

        public void DeleteSync(FinancialGoalEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute("usp_FinancialGoal_Delete", new
            {
                entity.Id
            }, commandType: CommandType.StoredProcedure);
        }

        #region Methods for filtering financial goal

        public async Task<IEnumerable<FinancialGoalEntity>> GetAllFilteredAsync(string filterBy, string value)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filter by field");

            var procedureName = GetFilteredProcedureName(filterBy);

            var parameterName = _orderMap[filterBy];

            var parameters = new DynamicParameters();

            parameters.Add(parameterName, value);

            return await connection.QueryAsync<FinancialGoalEntity>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<FinancialGoalEntity> GetAllFilteredSync(string filterBy, string value)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filter by field");

            var procedureName = GetFilteredProcedureName(filterBy);

            var parameterName = _orderMap[filterBy];

            var parameters = new DynamicParameters();

            parameters.Add(parameterName, value);

            return connection.Query<FinancialGoalEntity>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Methods for ordering financial goal

        public async Task<IEnumerable<FinancialGoalEntity>> GetAllOrderedAsync(string orderBy, string direction)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);

            var procedureName = GetProcedureName(orderBy, direction);

            return await connection.QueryAsync<FinancialGoalEntity>(procedureName, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<FinancialGoalEntity> GetAllOrderedSync(string orderBy, string direction)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);

            var procedureName = GetProcedureName(orderBy, direction);

            return connection.Query<FinancialGoalEntity>(procedureName, commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Additional Methods Private

        private string GetFilteredProcedureName(string filterBy)
        {
            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filterBy field");

            return $"usp_FinancialGoal_GetAll_FilterBy_{_orderMap[filterBy]}";
        }

        private string GetProcedureName(string orderBy, string direction)
        {
            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);
            return $"usp_FinancialGoal_GetAll_OrderBy_{_orderMap[orderBy]}_{(isDesc ? "Desc" : "Asc")}";
        }

        private static readonly Dictionary<string, string> _orderMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Name", "Name" }
        };

        #endregion
    }
}
