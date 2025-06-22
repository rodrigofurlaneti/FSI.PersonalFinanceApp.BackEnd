using Dapper;
using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FSI.PersonalFinanceApp.Infrastructure.Context;
using System.Data;

namespace FSI.PersonalFinanceApp.Infrastructure.Repositories
{
    public class ExpenseCategoryRepository : BaseRepository, IExpenseCategoryRepository
    {
        public ExpenseCategoryRepository(IDbContext context) : base(context) { }
        public async Task<IEnumerable<ExpenseCategoryEntity>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ExpenseCategoryEntity>(
                "usp_ExpenseCategory_GetAll",
                commandType: CommandType.StoredProcedure
            );
        }

        public IEnumerable<ExpenseCategoryEntity> GetAllSync()
        {
            using var connection = CreateConnection();
            return connection.Query<ExpenseCategoryEntity>(
                "usp_ExpenseCategory_GetAll",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<ExpenseCategoryEntity?> GetByIdAsync(long id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ExpenseCategoryEntity>(
                "usp_ExpenseCategory_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public ExpenseCategoryEntity? GetByIdSync(long id)
        {
            using var connection = CreateConnection();
            return connection.QueryFirstOrDefault<ExpenseCategoryEntity>(
                "usp_ExpenseCategory_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<long> AddAsync(ExpenseCategoryEntity entity)
        {
            using var connection = CreateConnection();

            var id = await connection.ExecuteScalarAsync<long>(
                "usp_ExpenseCategory_Add",
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

        public long AddSync(ExpenseCategoryEntity entity)
        {
            using var connection = CreateConnection();

            var id = connection.ExecuteScalar<long>(
                "usp_ExpenseCategory_Add",
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

        public async Task UpdateAsync(ExpenseCategoryEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_ExpenseCategory_Update",
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

        public void UpdateSync(ExpenseCategoryEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute(
                "usp_ExpenseCategory_Update",
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

        public async Task DeleteAsync(ExpenseCategoryEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_ExpenseCategory_Delete",
                new
                {
                    entity.Id
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteSync(ExpenseCategoryEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute("usp_ExpenseCategory_Delete", new
            {
                entity.Id
            }, commandType: CommandType.StoredProcedure);
        }

        #region Methods for filtering expenses categories

        public async Task<IEnumerable<ExpenseCategoryEntity>> GetAllFilteredAsync(string filterBy, string value)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filter by field");

            var procedureName = GetFilteredProcedureName(filterBy);

            var parameterName = _orderMap[filterBy];

            var parameters = new DynamicParameters();

            parameters.Add(parameterName, value);

            return await connection.QueryAsync<ExpenseCategoryEntity>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<ExpenseCategoryEntity> GetAllFilteredSync(string filterBy, string value)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filter by field");

            var procedureName = GetFilteredProcedureName(filterBy);

            var parameterName = _orderMap[filterBy];

            var parameters = new DynamicParameters();

            parameters.Add(parameterName, value);

            return connection.Query<ExpenseCategoryEntity>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Methods for ordering expenses categories

        public async Task<IEnumerable<ExpenseCategoryEntity>> GetAllOrderedAsync(string orderBy, string direction)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);

            var procedureName = GetProcedureName(orderBy, direction);

            return await connection.QueryAsync<ExpenseCategoryEntity>(procedureName, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<ExpenseCategoryEntity> GetAllOrderedSync(string orderBy, string direction)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);

            var procedureName = GetProcedureName(orderBy, direction);

            return connection.Query<ExpenseCategoryEntity>(procedureName, commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Additional Methods Private

        private string GetFilteredProcedureName(string filterBy)
        {
            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filter by field");

            return $"usp_ExpenseCategory_GetAll_FilterBy_{_orderMap[filterBy]}";
        }

        private string GetProcedureName(string orderBy, string direction)
        {
            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);
            return $"usp_ExpenseCategory_GetAll_OrderBy_{_orderMap[orderBy]}_{(isDesc ? "Desc" : "Asc")}";
        }

        private static readonly Dictionary<string, string> _orderMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Name", "Name" }
        };

        #endregion
    }

}
