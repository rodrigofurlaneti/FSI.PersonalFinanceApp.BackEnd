using Dapper;
using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FSI.PersonalFinanceApp.Infrastructure.Context;
using System.Data;

namespace FSI.PersonalFinanceApp.Infrastructure.Repositories
{
    public class TransactionRepository : BaseRepository, ITransactionRepository
    {
        public TransactionRepository(IDbContext context) : base(context) { }
        public async Task<IEnumerable<TransactionEntity>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<TransactionEntity>(
                "usp_Transaction_GetAll",
                commandType: CommandType.StoredProcedure
            );
        }

        public IEnumerable<TransactionEntity> GetAllSync()
        {
            using var connection = CreateConnection();
            return connection.Query<TransactionEntity>(
                "usp_Transaction_GetAll",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<TransactionEntity?> GetByIdAsync(long id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<TransactionEntity>(
                "usp_Transaction_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public TransactionEntity? GetByIdSync(long id)
        {
            using var connection = CreateConnection();
            return connection.QueryFirstOrDefault<TransactionEntity>(
                "usp_Transaction_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task AddAsync(TransactionEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_Transaction_Add",
                new
                {
                    entity.Name,
                    entity.AccountFromId,
                    entity.AccountToId,
                    entity.ExpenseId,
                    entity.IncomeId,
                    entity.TransactionDate,
                    entity.Amount,
                    entity.Description,
                    entity.IsActive,
                    entity.CreatedAt,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void AddSync(TransactionEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute(
                "usp_Transaction_Add",
                new
                {
                    entity.Name,
                    entity.AccountFromId,
                    entity.AccountToId,
                    entity.ExpenseId,
                    entity.IncomeId,
                    entity.TransactionDate,
                    entity.Amount,
                    entity.Description,
                    entity.IsActive,
                    entity.CreatedAt,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateAsync(TransactionEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_Transaction_Update",
                new
                {
                    entity.Id,
                    entity.Name,
                    entity.AccountFromId,
                    entity.AccountToId,
                    entity.ExpenseId,
                    entity.IncomeId,
                    entity.TransactionDate,
                    entity.Amount,
                    entity.Description,
                    entity.IsActive,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void UpdateSync(TransactionEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute(
                "usp_Transaction_Update",
                new
                {
                    entity.Id,
                    entity.Name,
                    entity.AccountFromId,
                    entity.AccountToId,
                    entity.ExpenseId,
                    entity.IncomeId,
                    entity.TransactionDate,
                    entity.Amount,
                    entity.Description,
                    entity.IsActive,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeleteAsync(TransactionEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "usp_Transaction_Delete",
                new
                {
                    entity.Id,
                    entity.Name,
                    entity.AccountFromId,
                    entity.AccountToId,
                    entity.ExpenseId,
                    entity.IncomeId,
                    entity.TransactionDate,
                    entity.Amount,
                    entity.Description,
                    entity.IsActive,
                    entity.UpdatedAt
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteSync(TransactionEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute("usp_Transaction_Delete", new
            {
                entity.Id
            }, commandType: CommandType.StoredProcedure);
        }

        #region Methods for filtering transaction

        public async Task<IEnumerable<TransactionEntity>> GetAllFilteredAsync(string filterBy, string value)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filter by field");

            var procedureName = GetFilteredProcedureName(filterBy);

            var parameterName = _orderMap[filterBy];

            var parameters = new DynamicParameters();

            parameters.Add(parameterName, value);

            return await connection.QueryAsync<TransactionEntity>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<TransactionEntity> GetAllFilteredSync(string filterBy, string value)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filter by field");

            var procedureName = GetFilteredProcedureName(filterBy);

            var parameterName = _orderMap[filterBy];

            var parameters = new DynamicParameters();

            parameters.Add(parameterName, value);

            return connection.Query<TransactionEntity>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Methods for ordering expenses

        public async Task<IEnumerable<TransactionEntity>> GetAllOrderedAsync(string orderBy, string direction)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);

            var procedureName = GetProcedureName(orderBy, direction);

            return await connection.QueryAsync<TransactionEntity>(procedureName, commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<TransactionEntity> GetAllOrderedSync(string orderBy, string direction)
        {
            using var connection = CreateConnection();

            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);

            var procedureName = GetProcedureName(orderBy, direction);

            return connection.Query<TransactionEntity>(procedureName, commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Additional Methods Private

        private string GetFilteredProcedureName(string filterBy)
        {
            if (!_orderMap.ContainsKey(filterBy))
                throw new ArgumentException("Invalid filterBy field");

            return $"usp_Transaction_GetAll_FilterBy_{_orderMap[filterBy]}";
        }

        private string GetProcedureName(string orderBy, string direction)
        {
            if (!_orderMap.ContainsKey(orderBy))
                throw new ArgumentException("Invalid orderBy field");

            var isDesc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);
            return $"usp_Transaction_GetAll_OrderBy_{_orderMap[orderBy]}_{(isDesc ? "Desc" : "Asc")}";
        }

        private static readonly Dictionary<string, string> _orderMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Name", "Name" }
        };

        #endregion

    }
}
