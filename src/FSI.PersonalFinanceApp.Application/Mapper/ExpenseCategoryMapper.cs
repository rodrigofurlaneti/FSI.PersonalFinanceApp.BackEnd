using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Application.Dtos;

namespace FSI.PersonalFinanceApp.Application.Mapper
{
    public static class ExpenseCategoryMapper
    {
        public static ExpenseCategoryEntity ToEntity(ExpenseCategoryDto dto)
        {
            return new ExpenseCategoryEntity
            {
                Id = dto.Id,
                Name = dto.Name,
                CreatedAt = dto.CreatedAt ?? DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = dto.IsActive
            };
        }

        public static ExpenseCategoryDto ToDto(ExpenseCategoryEntity entity)
        {
            return new ExpenseCategoryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                IsActive = entity.IsActive
            };
        }
    }
}
