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

        public async Task AddAsync(IncomeDto dto)
        {
            var entity = IncomeMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
        }

        public void AddSync(IncomeDto dto)
        {
            var entity = IncomeMapper.ToEntity(dto);
            _repository.AddSync(entity);
        }

        public async Task UpdateAsync(IncomeDto dto)
        {
            var entity = IncomeMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }

        public void UpdateSync(IncomeDto dto)
        {
            var entity = IncomeMapper.ToEntity(dto);
            _repository.UpdateSync(entity);
        }

        public async Task DeleteAsync(IncomeDto dto)
        {
            var entity = IncomeMapper.ToEntity(dto);
            await _repository.DeleteAsync(entity);
        }
    }

}
