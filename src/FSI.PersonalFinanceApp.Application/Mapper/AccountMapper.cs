using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Application.Dtos;

namespace FSI.PersonalFinanceApp.Application.Mapper
{
    public static class AccountMapper
    {
        public static AccountEntity ToEntity(AccountDto dto)
        {
            return new AccountEntity
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = dto.CreatedAt ?? DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = dto.IsActive
            };
        }

        public static AccountDto ToDto(AccountEntity entity)
        {
            return new AccountDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Balance = entity.Balance,
                Description = entity.Description,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                IsActive = entity.IsActive
            };
        }
    }
}
