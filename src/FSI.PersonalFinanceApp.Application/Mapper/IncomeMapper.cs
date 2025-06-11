using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Application.Dtos;

namespace FSI.PersonalFinanceApp.Application.Mapper
{
    public static class IncomeMapper
    {
        public static IncomeEntity ToEntity(IncomeDto dto)
        {
            return new IncomeEntity
            {
                Id = dto.Id,
                Name = dto.Name,
                Amount = dto.Amount,
                IncomeDate = dto.IncomeDate,
                Description = dto.Description,
                ReceivedAt = dto.ReceivedAt,
                IncomeCategoryId = dto.IncomeCategoryId,
                CreatedAt = dto.CreatedAt ?? DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = dto.IsActive
            };
        }

        public static IncomeDto ToDto(IncomeEntity entity)
        {
            return new IncomeDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Amount = entity.Amount,
                IncomeDate = entity.IncomeDate,
                Description = entity.Description,
                ReceivedAt = entity.ReceivedAt,
                IncomeCategoryId = entity.IncomeCategoryId,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                IsActive = entity.IsActive
            };
        }
    }
}
