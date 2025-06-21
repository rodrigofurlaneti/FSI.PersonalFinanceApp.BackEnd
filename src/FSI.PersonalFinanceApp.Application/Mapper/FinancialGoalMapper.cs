using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Domain.Entities;

namespace FSI.PersonalFinanceApp.Application.Mapper
{
    public static class FinancialGoalMapper
    {
        public static FinancialGoalEntity ToEntity(FinancialGoalDto dto)
        {
            return new FinancialGoalEntity
            {
                Id = dto.Id,
                Name = dto.Name,
                TargetAmount = dto.TargetAmount,
                CurrentAmount = dto.CurrentAmount,
                DueDate = dto.DueDate,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt ?? DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }

        public static FinancialGoalDto ToDto(FinancialGoalEntity entity)
        {
            return new FinancialGoalDto
            {
                Id = entity.Id,
                Name = entity.Name,
                TargetAmount = entity.TargetAmount,
                CurrentAmount = entity.CurrentAmount,
                DueDate = entity.DueDate,
                Description = entity.Description,
                IsCompleted = entity.IsCompleted,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
}
