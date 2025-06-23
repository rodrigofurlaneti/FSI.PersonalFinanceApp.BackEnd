using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Application.Mapper;
using FSI.PersonalFinanceApp.Domain.Interfaces;

namespace FSI.PersonalFinanceApp.Application.Services
{
    public class IncomeAppService : IIncomeAppService
    {
        private readonly IIncomeRepository _repository;

        public IncomeAppService(IIncomeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<IncomeDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(IncomeMapper.ToDto);
        }

        public IEnumerable<IncomeDto> GetAllSync()
        {
            var entities = _repository.GetAllSync();
            return entities.Select(IncomeMapper.ToDto);
        }

        public async Task<IncomeDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : IncomeMapper.ToDto(entity);
        }

        public IncomeDto? GetByIdSync(long id)
        {
            var entity = _repository.GetByIdSync(id);
            return entity is null ? null : IncomeMapper.ToDto(entity);
        }

        public async Task<long> AddAsync(IncomeDto dto)
        {
            var entity = IncomeMapper.ToEntity(dto);
            return await _repository.AddAsync(entity);
        }

        public long AddSync(IncomeDto dto)
        {
            var entity = IncomeMapper.ToEntity(dto);
            return _repository.AddSync(entity);
        }

        public async Task<bool> UpdateAsync(IncomeDto dto)
        {
            var entity = IncomeMapper.ToEntity(dto);
            return await _repository.UpdateAsync(entity);
        }

        public bool UpdateSync(IncomeDto dto)
        {
            var entity = IncomeMapper.ToEntity(dto);
            return _repository.UpdateSync(entity);
        }

        public async Task<bool> DeleteAsync(IncomeDto dto)
        {
            var entity = IncomeMapper.ToEntity(dto);
            return await _repository.DeleteAsync(entity);
        }

        public bool DeleteSync(IncomeDto dto)
        {
            var entity = IncomeMapper.ToEntity(dto);
            return _repository.DeleteSync(entity);
        }
        
        public async Task<IEnumerable<IncomeDto>> GetAllFilteredAsync(string filterBy, string value)
        {
            var entities = await _repository.GetAllFilteredAsync(filterBy, value);
            return entities.Select(IncomeMapper.ToDto);
        }

        public IEnumerable<IncomeDto> GetAllFilteredSync(string filterBy, string value)
        {
            var entities = _repository.GetAllFilteredSync(filterBy, value);
            return entities.Select(IncomeMapper.ToDto);
        }

        public async Task<IEnumerable<IncomeDto>> GetAllOrderedAsync(string orderBy, string direction)
        {
            var entities = await _repository.GetAllOrderedAsync(orderBy, direction);
            return entities.Select(IncomeMapper.ToDto);
        }

        public IEnumerable<IncomeDto> GetAllOrderedSync(string orderBy, string direction)
        {
            var entities = _repository.GetAllOrderedSync(orderBy, direction);
            return entities.Select(IncomeMapper.ToDto);
        }
    }

}
