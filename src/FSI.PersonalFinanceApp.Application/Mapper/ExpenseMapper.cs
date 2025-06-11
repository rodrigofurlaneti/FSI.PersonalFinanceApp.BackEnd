using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Domain.Entities;

namespace FSI.PersonalFinanceApp.Application.Mapper
{
    public static class ExpenseMapper
    {
        public static ExpenseEntity ToEntity(ExpenseDto dto)
        {
            return new ExpenseEntity
            {
                Id = dto.Id,
                Name = dto.Name,
                Amount = dto.Amount,
                DueDate = dto.DueDate,
                Description = dto.Description,
                PaidAt = dto.PaidAt,
                ExpenseCategoryId = dto.ExpenseCategoryId,
                CreatedAt = dto.CreatedAt ?? DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = dto.IsActive
            };
        }

        public static ExpenseDto ToDto(ExpenseEntity entity)
        {
            return new ExpenseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Amount = entity.Amount,
                DueDate = entity.DueDate,
                Description = entity.Description,
                PaidAt = entity.PaidAt,
                ExpenseCategoryId = entity.ExpenseCategoryId,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                IsActive = entity.IsActive
            };
        }
    }
}
