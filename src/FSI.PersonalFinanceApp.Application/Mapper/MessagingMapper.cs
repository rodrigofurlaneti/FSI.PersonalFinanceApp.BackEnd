using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Domain.Entities;

namespace FSI.PersonalFinanceApp.Application.Mapper
{
    public static class MessagingMapper
    {
        public static MessagingEntity ToEntity(MessagingDto dto)
        {
            return new MessagingEntity
            {
                Id = dto.Id,
                Action = dto.Action,
                QueueName = dto.QueueName,
                MessageContent = dto.MessageContent,
                IsProcessed = dto.IsProcessed,
                ErrorMessage = dto.ErrorMessage,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = dto.IsActive
            };
        }

        public static MessagingDto ToDto(MessagingEntity entity)
        {
            return new MessagingDto
            {
                Id = entity.Id,
                QueueName = entity.QueueName,
                MessageContent = entity.MessageContent,
                IsProcessed = entity.IsProcessed,
                ErrorMessage = entity.ErrorMessage,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = DateTime.Now,
                IsActive = entity.IsActive
            };
        }
    }
}
