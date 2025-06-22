using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Application.Mapper;
using FSI.PersonalFinanceApp.Domain.Interfaces;

namespace FSI.PersonalFinanceApp.Application.Services
{
    public class AccountAppService : IAccountAppService
    {
        private readonly IAccountRepository _repository;

        public AccountAppService(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AccountDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(AccountMapper.ToDto);
        }

        public IEnumerable<AccountDto> GetAllSync()
        {
            var entities = _repository.GetAllSync();
            return entities.Select(AccountMapper.ToDto);
        }

        public async Task<AccountDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : AccountMapper.ToDto(entity);
        }

        public AccountDto? GetByIdSync(long id)
        {
            var entity = _repository.GetByIdSync(id);
            return entity is null ? null : AccountMapper.ToDto(entity);
        }

        public async Task<long> AddAsync(AccountDto dto)
        {
            var entity = AccountMapper.ToEntity(dto);
            return await _repository.AddAsync(entity);
        }

        public long AddSync(AccountDto dto)
        {
            var entity = AccountMapper.ToEntity(dto);
            return _repository.AddSync(entity);
        }

        public async Task UpdateAsync(AccountDto dto)
        {
            var entity = AccountMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }

        public void UpdateSync(AccountDto dto)
        {
            var entity = AccountMapper.ToEntity(dto);
            _repository.UpdateSync(entity);
        }

        public async Task DeleteAsync(AccountDto dto)
        {
            var entity = AccountMapper.ToEntity(dto);
            await _repository.DeleteAsync(entity);
        }

        public void DeleteSync(AccountDto dto)
        {
            var entity = AccountMapper.ToEntity(dto);
            _repository.DeleteSync(entity);
        }

        public async Task<IEnumerable<AccountDto>> GetAllFilteredAsync(string filterBy, string value)
        {
            var entities = await _repository.GetAllFilteredAsync(filterBy, value);
            return entities.Select(AccountMapper.ToDto);
        }

        public IEnumerable<AccountDto> GetAllFilteredSync(string filterBy, string value)
        {
            var entities = _repository.GetAllFilteredSync(filterBy, value);
            return entities.Select(AccountMapper.ToDto);
        }

        public async Task<IEnumerable<AccountDto>> GetAllOrderedAsync(string orderBy, string direction)
        {
            var entities = await _repository.GetAllOrderedAsync(orderBy, direction);
            return entities.Select(AccountMapper.ToDto);
        }

        public IEnumerable<AccountDto> GetAllOrderedSync(string orderBy, string direction)
        {
            var entities = _repository.GetAllOrderedSync(orderBy, direction);
            return entities.Select(AccountMapper.ToDto);
        }
    }
}
