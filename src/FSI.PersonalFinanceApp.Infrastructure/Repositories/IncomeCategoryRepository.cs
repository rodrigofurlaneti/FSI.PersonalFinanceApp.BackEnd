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

        public async Task<long> AddAsync(IncomeCategoryEntity entity)
        {
            using var connection = CreateConnection();

            var id = await connection.ExecuteScalarAsync<long>("usp_IncomeCategory_Add",
                new
                {
                    entity.Name,
                    entity.IsActive,
                    entity.CreatedAt,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );

            return id;
        }

        public long AddSync(IncomeCategoryEntity entity)
        {
            using var connection = CreateConnection();

            var id = connection.ExecuteScalar<long>("usp_IncomeCategory_Add",
                new
                {
                    entity.Name,
                    entity.IsActive,
                    entity.CreatedAt,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );

            return id;
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
                    entity.Id
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteSync(IncomeCategoryEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute("usp_IncomeCategory_Delete", new
            {
                entity.Id
            }, commandType: CommandType.StoredProcedure);
        }

        #region Methods for filtering income category

        public async Task<IEnumerable<IncomeCategoryEntity>> GetAllFilteredAsync(string filterBy, string value)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filter by field");

            var procedureName = GetFilteredProcedureName(filterBy);

            var parameterName = _orderMap[filterBy];

            var parameters = new DynamicParameters();

            parameters.Add(parameterName, value);

            return await connection.QueryAsync<IncomeCategoryEntity>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<IncomeCategoryEntity> GetAllFilteredSync(string filterBy, string value)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filter by field");

            var procedureName = GetFilteredProcedureName(filterBy);

            var parameterName = _orderMap[filterBy];

            var parameters = new DynamicParameters();

            parameters.Add(parameterName, value);

            return connection.Query<IncomeCategoryEntity>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Methods for ordering expenses

        public async Task<IEnumerable<IncomeCategoryEntity>> GetAllOrderedAsync(string orderBy, string direction)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);

            var procedureName = GetProcedureName(orderBy, direction);

            return await connection.QueryAsync<IncomeCategoryEntity>(procedureName, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<IncomeCategoryEntity> GetAllOrderedSync(string orderBy, string direction)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);

            var procedureName = GetProcedureName(orderBy, direction);

            return connection.Query<IncomeCategoryEntity>(procedureName, commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Additional Methods Private

        private string GetFilteredProcedureName(string filterBy)
        {
            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filter by field");

            return $"usp_IncomeCategory_GetAll_FilterBy_{_orderMap[filterBy]}";
        }

        private string GetProcedureName(string orderBy, string direction)
        {
            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);
            return $"usp_IncomeCategory_GetAll_OrderBy_{_orderMap[orderBy]}_{(isDesc ? "Desc" : "Asc")}";
        }

        private static readonly Dictionary<string, string> _orderMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Name", "Name" }
        };

        #endregion
    }
}
