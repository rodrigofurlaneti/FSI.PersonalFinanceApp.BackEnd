using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Application.Mapper;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.PersonalFinanceApp.Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _repository;

        public UserAppService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(UserMapper.ToDto);
        }

        public IEnumerable<UserDto> GetAllSync()
        {
            var entities = _repository.GetAllSync();
            return entities.Select(UserMapper.ToDto);
        }

        public async Task<UserDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : UserMapper.ToDto(entity);
        }

        public UserDto? GetByIdSync(long id)
        {
            var entity = _repository.GetByIdSync(id);
            return entity is null ? null : UserMapper.ToDto(entity);
        }

        public async Task<long> AddAsync(UserDto dto)
        {
            var entity = UserMapper.ToEntity(dto);
            return await _repository.AddAsync(entity);
        }

        public long AddSync(UserDto dto)
        {
            var entity = UserMapper.ToEntity(dto);
            return _repository.AddSync(entity);
        }

        public async Task<bool> UpdateAsync(UserDto dto)
        {
            var entity = UserMapper.ToEntity(dto);
            return await _repository.UpdateAsync(entity);
        }

        public bool UpdateSync(UserDto dto)
        {
            var entity = UserMapper.ToEntity(dto);
            return _repository.UpdateSync(entity);
        }

        public async Task<bool> DeleteAsync(UserDto dto)
        {
            var entity = UserMapper.ToEntity(dto);
            return await _repository.DeleteAsync(entity);
        }

        public bool DeleteSync(UserDto dto)
        {
            var entity = UserMapper.ToEntity(dto);
            return _repository.DeleteSync(entity);
        }

        public async Task<IEnumerable<UserDto>> GetAllFilteredAsync(string filterBy, string value)
        {
            var entities = await _repository.GetAllFilteredAsync(filterBy, value);
            return entities.Select(UserMapper.ToDto);
        }

        public IEnumerable<UserDto> GetAllFilteredSync(string filterBy, string value)
        {
            var entities = _repository.GetAllFilteredSync(filterBy, value);
            return entities.Select(UserMapper.ToDto);
        }

        public async Task<IEnumerable<UserDto>> GetAllOrderedAsync(string orderBy, string direction)
        {
            var entities = await _repository.GetAllOrderedAsync(orderBy, direction);
            return entities.Select(UserMapper.ToDto);
        }

        public IEnumerable<UserDto> GetAllOrderedSync(string orderBy, string direction)
        {
            var entities = _repository.GetAllOrderedSync(orderBy, direction);
            return entities.Select(UserMapper.ToDto);
        }
    }
}
