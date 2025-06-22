using Dapper;
using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using FSI.PersonalFinanceApp.Infrastructure.Context;
using System.Data;
using static Dapper.SqlMapper;

namespace FSI.PersonalFinanceApp.Infrastructure.Repositories
{
    public class MessagingRepository : BaseRepository, IMessagingRepository
    {
        public MessagingRepository(IDbContext context) : base(context) { }

        #region Methods based on IMessagingRepository interface

        public async Task<IEnumerable<MessagingEntity>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<MessagingEntity>("usp_Messaging_GetAll", commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<MessagingEntity> GetAllSync()
        {
            using var connection = CreateConnection();
            return connection.Query<MessagingEntity>("usp_Messaging_GetAll", commandType: CommandType.StoredProcedure);
        }

        public async Task<MessagingEntity?> GetByIdAsync(long id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<MessagingEntity>(
                "usp_Messaging_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public MessagingEntity? GetByIdSync(long id)
        {
            using var connection = CreateConnection();
            return connection.QueryFirstOrDefault<MessagingEntity>(
                "usp_Messaging_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<long> AddAsync(MessagingEntity entity)
        {
            using var connection = CreateConnection();

            var id = await connection.ExecuteScalarAsync<long>("usp_Messaging_Add", new
            {
                entity.Action,
                entity.QueueName,
                entity.MessageContent,
                entity.IsProcessed,
                entity.ErrorMessage,
                entity.IsActive,
                entity.CreatedAt,
                entity.UpdatedAt
            }, commandType: CommandType.StoredProcedure);

            return id;
        }

        public long AddSync(MessagingEntity entity)
        {
            using var connection = CreateConnection();

            var id = connection.ExecuteScalar<long>("usp_Messaging_Add", new
            {
                entity.Action,
                entity.QueueName,
                entity.MessageContent,
                entity.IsProcessed,
                entity.ErrorMessage,
                entity.IsActive,
                entity.CreatedAt,
                entity.UpdatedAt
            }, commandType: CommandType.StoredProcedure);

            return id;
        }

        public async Task UpdateAsync(MessagingEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("usp_Messaging_Update", new
            {
                entity.Id,
                entity.Action,
                entity.QueueName,
                entity.MessageContent,
                entity.IsProcessed,
                entity.ErrorMessage,
                entity.IsActive,
                entity.UpdatedAt
            }, commandType: CommandType.StoredProcedure);
        }

        public void UpdateSync(MessagingEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute("usp_Messaging_Update", new
            {
                entity.Id,
                entity.Action,
                entity.QueueName,
                entity.MessageContent,
                entity.IsProcessed,
                entity.ErrorMessage,
                entity.IsActive,
                entity.UpdatedAt
            }, commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteAsync(MessagingEntity entity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("usp_Messaging_Delete", new
            {
                entity.Id
            }, commandType: CommandType.StoredProcedure);
        }

        public void DeleteSync(MessagingEntity entity)
        {
            using var connection = CreateConnection();
            connection.Execute("usp_Messaging_Delete", new
            {
                entity.Id
            }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<MessagingEntity>> GetAllFilteredAsync(string filterBy, string value)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MessagingEntity> GetAllFilteredSync(string filterBy, string value)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MessagingEntity>> GetAllOrderedAsync(string orderBy, string direction)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MessagingEntity> GetAllOrderedSync(string orderBy, string direction)
        {
            throw new NotImplementedException();
        }
        public async Task MarkAsProcessedAsync(long id)
        {
            using var connection = CreateConnection();

            var sql = @"UPDATE Messaging SET IsProcessed = 1, UpdatedAt = GETDATE() WHERE Id = @Id";

            await connection.ExecuteAsync(sql, new { Id = id });
        }


        #endregion
    }
}
