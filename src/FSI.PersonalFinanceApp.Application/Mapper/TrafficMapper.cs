using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Domain.Entities;

namespace FSI.PersonalFinanceApp.Application.Mapper
{
    public static class TrafficMapper
    {
        // Assuming TrafficDto and TrafficEntity are defined elsewhere in the project
        public static TrafficEntity ToEntity(TrafficDto dto)
        {
            return new TrafficEntity
            {
                Id = dto.Id,
                Method = dto.Method,
                Action = dto.Action,
                BackEndCreatedAt = dto.BackEndCreatedAt ?? DateTime.Now,
                CreatedAt = dto.CreatedAt ?? DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = dto.IsActive
            };
        }
        public static TrafficDto ToDto(TrafficEntity entity)
        {
            return new TrafficDto
            {
                Id = entity.Id,
                Method = entity.Method,
                Action = entity.Action,
                BackEndCreatedAt = entity.BackEndCreatedAt ?? DateTime.Now,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                IsActive = entity.IsActive
            };
        }
    }
}
