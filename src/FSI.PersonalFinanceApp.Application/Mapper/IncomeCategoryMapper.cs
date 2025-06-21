using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Application.Dtos;

namespace FSI.PersonalFinanceApp.Application.Mapper
{
    public static class IncomeCategoryMapper
    {
        public static IncomeCategoryEntity ToEntity(IncomeCategoryDto dto)
        {
            return new IncomeCategoryEntity
            {
                Id = dto.Id,
                Name = dto.Name,
                CreatedAt = dto.CreatedAt ?? DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = dto.IsActive
            };
        }

        public static IncomeCategoryDto ToDto(IncomeCategoryEntity entity)
        {
            return new IncomeCategoryDto
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
