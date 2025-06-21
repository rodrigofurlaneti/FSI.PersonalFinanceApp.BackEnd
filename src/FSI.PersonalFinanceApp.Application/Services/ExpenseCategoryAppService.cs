using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Application.Mapper;
using FSI.PersonalFinanceApp.Domain.Interfaces;

namespace FSI.PersonalFinanceApp.Application.Services
{
    public class ExpenseCategoryAppService : IExpenseCategoryAppService
    {
        private readonly IExpenseCategoryRepository _repository;

        public ExpenseCategoryAppService(IExpenseCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ExpenseCategoryDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(ExpenseCategoryMapper.ToDto);
        }

        public IEnumerable<ExpenseCategoryDto> GetAllSync()
        {
            var entities = _repository.GetAllSync();
            return entities.Select(ExpenseCategoryMapper.ToDto);
        }

        public async Task<ExpenseCategoryDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : ExpenseCategoryMapper.ToDto(entity);
        }

        public ExpenseCategoryDto? GetByIdSync(long id)
        {
            var entity = _repository.GetByIdSync(id);
            return entity is null ? null : ExpenseCategoryMapper.ToDto(entity);
        }

        public async Task AddAsync(ExpenseCategoryDto dto)
        {
            var entity = ExpenseCategoryMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
        }

        public void AddSync(ExpenseCategoryDto dto)
        {
            var entity = ExpenseCategoryMapper.ToEntity(dto);
            _repository.AddSync(entity);
        }

        public async Task UpdateAsync(ExpenseCategoryDto dto)
        {
            var entity = ExpenseCategoryMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }

        public void UpdateSync(ExpenseCategoryDto dto)
        {
            var entity = ExpenseCategoryMapper.ToEntity(dto);
            _repository.UpdateSync(entity);
        }

        public async Task DeleteAsync(ExpenseCategoryDto dto)
        {
            var entity = ExpenseCategoryMapper.ToEntity(dto);
            await _repository.DeleteAsync(entity);
        }
        public void DeleteSync(ExpenseCategoryDto dto)
        {
            var entity = ExpenseCategoryMapper.ToEntity(dto);
            _repository.DeleteSync(entity);
        }

        public async Task<IEnumerable<ExpenseCategoryDto>> GetAllFilteredAsync(string filterBy, string value)
        {
            var entities = await _repository.GetAllFilteredAsync(filterBy, value);
            return entities.Select(ExpenseCategoryMapper.ToDto);
        }

        public IEnumerable<ExpenseCategoryDto> GetAllFilteredSync(string filterBy, string value)
        {
            var entities = _repository.GetAllFilteredSync(filterBy, value);
            return entities.Select(ExpenseCategoryMapper.ToDto);
        }

        public async Task<IEnumerable<ExpenseCategoryDto>> GetAllOrderedAsync(string orderBy, string direction)
        {
            var entities = await _repository.GetAllOrderedAsync(orderBy, direction);
            return entities.Select(ExpenseCategoryMapper.ToDto);
        }

        public IEnumerable<ExpenseCategoryDto> GetAllOrderedSync(string orderBy, string direction)
        {
            var entities = _repository.GetAllOrderedSync(orderBy, direction);
            return entities.Select(ExpenseCategoryMapper.ToDto);
        }
    }

}
