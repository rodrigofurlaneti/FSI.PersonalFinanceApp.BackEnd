using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Domain.Entities;

namespace FSI.PersonalFinanceApp.Application.Mapper
{
    public static class UserMapper
    {
        public static UserEntity ToEntity(UserDto dto)
        {
            return new UserEntity
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = dto.PasswordHash,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt ?? DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }

        public static UserDto ToDto(UserEntity entity)
        {
            return new UserDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                PasswordHash = entity.PasswordHash,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
}
