using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Application.Mapper;
using FSI.PersonalFinanceApp.Domain.Interfaces;

namespace FSI.PersonalFinanceApp.Application.Services
{
    public class FinancialGoalAppService : IFinancialGoalAppService
    {
        private readonly IFinancialGoalRepository _repository;

        public FinancialGoalAppService(IFinancialGoalRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<FinancialGoalDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(FinancialGoalMapper.ToDto);
        }

        public IEnumerable<FinancialGoalDto> GetAllSync()
        {
            var entities = _repository.GetAllSync();
            return entities.Select(FinancialGoalMapper.ToDto);
        }

        public async Task<FinancialGoalDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : FinancialGoalMapper.ToDto(entity);
        }

        public FinancialGoalDto? GetByIdSync(long id)
        {
            var entity = _repository.GetByIdSync(id);
            return entity is null ? null : FinancialGoalMapper.ToDto(entity);
        }

        public async Task AddAsync(FinancialGoalDto dto)
        {
            var entity = FinancialGoalMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
        }

        public void AddSync(FinancialGoalDto dto)
        {
            var entity = FinancialGoalMapper.ToEntity(dto);
            _repository.AddSync(entity);
        }

        public async Task UpdateAsync(FinancialGoalDto dto)
        {
            var entity = FinancialGoalMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }

        public void UpdateSync(FinancialGoalDto dto)
        {
            var entity = FinancialGoalMapper.ToEntity(dto);
            _repository.UpdateSync(entity);
        }

        public async Task DeleteAsync(FinancialGoalDto dto)
        {
            var entity = FinancialGoalMapper.ToEntity(dto);
            await _repository.DeleteAsync(entity);
        }

        public void DeleteSync(FinancialGoalDto dto)
        {
            var entity = FinancialGoalMapper.ToEntity(dto);
            _repository.DeleteSync(entity);
        }

        public async Task<IEnumerable<FinancialGoalDto>> GetAllFilteredAsync(string filterBy, string value)
        {
            var entities = await _repository.GetAllFilteredAsync(filterBy, value);
            return entities.Select(FinancialGoalMapper.ToDto);
        }

        public IEnumerable<FinancialGoalDto> GetAllFilteredSync(string filterBy, string value)
        {
            var entities = _repository.GetAllFilteredSync(filterBy, value);
            return entities.Select(FinancialGoalMapper.ToDto);
        }

        public async Task<IEnumerable<FinancialGoalDto>> GetAllOrderedAsync(string orderBy, string direction)
        {
            var entities = await _repository.GetAllOrderedAsync(orderBy, direction);
            return entities.Select(FinancialGoalMapper.ToDto);
        }

        public IEnumerable<FinancialGoalDto> GetAllOrderedSync(string orderBy, string direction)
        {
            var entities = _repository.GetAllOrderedSync(orderBy, direction);
            return entities.Select(FinancialGoalMapper.ToDto);
        }
    }
}
