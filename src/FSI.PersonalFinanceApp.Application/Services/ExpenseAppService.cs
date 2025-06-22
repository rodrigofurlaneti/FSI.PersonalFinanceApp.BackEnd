using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Application.Mapper;
using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;

namespace FSI.PersonalFinanceApp.Application.Services
{
    public class ExpenseAppService : IExpenseAppService
    {
        private readonly IExpenseRepository _repository; // ✅ Declaração

        public ExpenseAppService(IExpenseRepository repository) // ✅ Construtor
        {
            _repository = repository;
        }

        #region Methods from IBaseAppService

        public async Task<IEnumerable<ExpenseDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(ExpenseMapper.ToDto);
        }

        public IEnumerable<ExpenseDto> GetAllSync()
        {
            var entities = _repository.GetAllSync();
            return entities.Select(ExpenseMapper.ToDto);
        }

        public async Task<ExpenseDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : ExpenseMapper.ToDto(entity);
        }

        public ExpenseDto? GetByIdSync(long id)
        {
            var entity = _repository.GetByIdSync(id);
            return entity is null ? null : ExpenseMapper.ToDto(entity);
        }

        public async Task<long> AddAsync(ExpenseDto dto)
        {
            var entity = ExpenseMapper.ToEntity(dto);
            return await _repository.AddAsync(entity);
        }

        public long AddSync(ExpenseDto dto)
        {
            var entity = ExpenseMapper.ToEntity(dto);
            return _repository.AddSync(entity);
        }

        public async Task<bool> UpdateAsync(ExpenseDto dto)
        {
            var entity = ExpenseMapper.ToEntity(dto);
            return await _repository.UpdateAsync(entity);
        }

        public bool UpdateSync(ExpenseDto dto)
        {
            var entity = ExpenseMapper.ToEntity(dto);
            return _repository.UpdateSync(entity);
        }

        public async Task<bool> DeleteAsync(ExpenseDto dto)
        {
            var entity = ExpenseMapper.ToEntity(dto);
            return await _repository.DeleteAsync(entity);
        }

        public bool DeleteSync(ExpenseDto dto)
        {
            var entity = ExpenseMapper.ToEntity(dto);
            return _repository.DeleteSync(entity);
        }

        public async Task<IEnumerable<ExpenseDto>> GetAllFilteredAsync(string filterBy, string value)
        {
            var entities = await _repository.GetAllFilteredAsync(filterBy, value);
            return entities.Select(ExpenseMapper.ToDto);
        }

        public IEnumerable<ExpenseDto> GetAllFilteredSync(string filterBy, string value)
        {
            var entities = _repository.GetAllFilteredSync(filterBy, value);
            return entities.Select(ExpenseMapper.ToDto);
        }

        public async Task<IEnumerable<ExpenseDto>> GetAllOrderedAsync(string orderBy, string direction)
        {
            var entities = await _repository.GetAllOrderedAsync(orderBy, direction);
            return entities.Select(ExpenseMapper.ToDto);
        }

        public IEnumerable<ExpenseDto> GetAllOrderedSync(string orderBy, string direction)
        {
            var entities = _repository.GetAllOrderedSync(orderBy, direction);
            return entities.Select(ExpenseMapper.ToDto);
        }

        #endregion

        #region Additional Methods

        #endregion
    }
}
