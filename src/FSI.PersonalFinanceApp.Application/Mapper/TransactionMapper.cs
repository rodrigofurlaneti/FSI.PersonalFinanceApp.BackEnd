using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Application.Dtos;

namespace FSI.PersonalFinanceApp.Application.Mapper
{
    public static class TransactionMapper
    {
        public static TransactionEntity ToEntity(TransactionDto dto)
        {
            return new TransactionEntity
            {
                Id = dto.Id,
                Name = dto.Name,
                AccountFromId = dto.AccountFromId,
                AccountToId = dto.AccountToId,
                ExpenseId = dto.ExpenseId,
                IncomeId = dto.IncomeId,
                TransactionDate = dto.TransactionDate,
                Amount = dto.Amount,
                Description = dto.Description,
                CreatedAt = dto.CreatedAt ?? DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = dto.IsActive
            };
        }

        public static TransactionDto ToDto(TransactionEntity entity)
        {
            return new TransactionDto
            {
                Id = entity.Id,
                Name = entity.Name,
                AccountFromId = entity.AccountFromId,
                AccountToId = entity.AccountToId,
                ExpenseId = entity.ExpenseId,
                IncomeId = entity.IncomeId,
                TransactionDate = entity.TransactionDate,
                Amount = entity.Amount,
                Description = entity.Description,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                IsActive = entity.IsActive
            };
        }
    }

}
