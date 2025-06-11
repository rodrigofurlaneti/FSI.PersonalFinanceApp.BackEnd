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

        public async Task AddAsync(TrafficEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("usp_Traffic_Add", new
            {
                entity.Method,
                entity.Action,
                entity.BackEndCreatedAt, 
                entity.IsActive,
                entity.CreatedAt,
                entity.UpdatedAt
            }, commandType: CommandType.StoredProcedure);
        }

        public void AddSync(TrafficEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute("usp_Traffic_Add", new
            {
                entity.Method,
                entity.Action,
                entity.BackEndCreatedAt,
                entity.IsActive,
                entity.CreatedAt,
                entity.UpdatedAt
            }, commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(TrafficEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("usp_Traffic_Update", new
            {
                entity.Id,
                entity.Method,
                entity.Action,
                entity.IsActive,
                entity.CreatedAt,
                entity.UpdatedAt
            }, commandType: CommandType.StoredProcedure);
        }

        public void UpdateSync(TrafficEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute("usp_Traffic_Update", new
            {
                entity.Id,
                entity.Method,
                entity.Action,
                entity.IsActive,
                entity.CreatedAt,
                entity.UpdatedAt
            }, commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteAsync(TrafficEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("usp_Traffic_Delete", new
            {
                entity.Id
            }, commandType: CommandType.StoredProcedure);
        }

        public void DeleteSync(TrafficEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute("usp_Traffic_Delete", new
            {
                entity.Id
            }, commandType: CommandType.StoredProcedure);
        }

        #endregion
    }
}