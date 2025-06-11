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

        public async Task AddAsync(ExpenseDto dto)
        {
            var entity = ExpenseMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
        }

        public void AddSync(ExpenseDto dto)
        {
            var entity = ExpenseMapper.ToEntity(dto);
            _repository.AddSync(entity);
        }

        public async Task UpdateAsync(ExpenseDto dto)
        {
            var entity = ExpenseMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }

        public void UpdateSync(ExpenseDto dto)
        {
            var entity = ExpenseMapper.ToEntity(dto);
            _repository.UpdateSync(entity);
        }

        public async Task DeleteAsync(ExpenseDto dto)
        {
            var entity = ExpenseMapper.ToEntity(dto);
            await _repository.DeleteAsync(entity);
        }

        public void DeleteSync(ExpenseDto dto)
        {
            var entity = ExpenseMapper.ToEntity(dto);
            _repository.DeleteSync(entity);
        }

        #endregion

        #region Additional Methods

        //Name Async
        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Name_Asc_Async()
        {
            var entities = await _repository.GetAll_Orderby_Name_Asc_Async();
            return entities;
        }

        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Name_Desc_Async()
        {
            var entities = await _repository.GetAll_Orderby_Name_Desc_Async();
            return entities;
        }

        //Name Sync
        public IEnumerable<ExpenseEntity> GetAll_Orderby_Name_Asc_Sync()
        {
            var entities = _repository.GetAll_Orderby_Name_Asc_Sync();
            return entities;
        }

        public IEnumerable<ExpenseEntity> GetAll_Orderby_Name_Desc_Sync()
        {
            var entities = _repository.GetAll_Orderby_Name_Desc_Sync();
            return entities;
        }

        //Description Async
        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Description_Asc_Async()
        {
            var entities = await _repository.GetAll_Orderby_Description_Asc_Async();
            return entities;
        }

        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Description_Desc_Async()
        {
            var entities = await _repository.GetAll_Orderby_Description_Desc_Async();
            return entities;
        }

        //Description Sync
        public IEnumerable<ExpenseEntity> GetAll_Orderby_Description_Asc_Sync()
        {
            var entities = _repository.GetAll_Orderby_Description_Asc_Sync();
            return entities;
        }

        public IEnumerable<ExpenseEntity> GetAll_Orderby_Description_Desc_Sync()
        {
            var entities = _repository.GetAll_Orderby_Description_Desc_Sync();
            return entities;
        }

        //PaidAt Async
        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_PaidAt_Desc_Async()
        {
            var entities = await _repository.GetAll_Orderby_PaidAt_Desc_Async();
            return entities;
        }

        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_PaidAt_Asc_Async()
        {
            var entities = await _repository.GetAll_Orderby_PaidAt_Asc_Async();
            return entities;
        }

        //PaidAt Sync
        public IEnumerable<ExpenseEntity> GetAll_Orderby_PaidAt_Desc_Sync()
        {
            var entities = _repository.GetAll_Orderby_PaidAt_Desc_Sync();
            return entities;
        }

        public IEnumerable<ExpenseEntity> GetAll_Orderby_PaidAt_Asc_Sync()
        {
            var entities = _repository.GetAll_Orderby_PaidAt_Asc_Sync();
            return entities;
        }

        //DueDate Async
        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_DueDate_Desc_Async()
        {
            var entities = await _repository.GetAll_Orderby_DueDate_Desc_Async();
            return entities;
        }

        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_DueDate_Asc_Async()
        {
            var entities = await _repository.GetAll_Orderby_DueDate_Asc_Async();
            return entities;
        }

        //DueDate Sync
        public IEnumerable<ExpenseEntity> GetAll_Orderby_DueDate_Desc_Sync()
        {
            var entities = _repository.GetAll_Orderby_DueDate_Desc_Sync();
            return entities;
        }

        public IEnumerable<ExpenseEntity> GetAll_Orderby_DueDate_Asc_Sync()
        {
            var entities = _repository.GetAll_Orderby_DueDate_Asc_Sync();
            return entities;
        }

        //Amount Async
         public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Amount_Desc_Async()
        {
            var entities = await _repository.GetAll_Orderby_Amount_Desc_Async();
            return entities;
        }

        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_Amount_Asc_Async()
        {
            var entities = await _repository.GetAll_Orderby_Amount_Asc_Async();
            return entities;
        }

        //Amount Sync
        public IEnumerable<ExpenseEntity> GetAll_Orderby_Amount_Desc_Sync()
        {
            var entities = _repository.GetAll_Orderby_Amount_Desc_Sync();
            return entities;
        }

        public IEnumerable<ExpenseEntity> GetAll_Orderby_Amount_Asc_Sync()
        {
            var entities = _repository.GetAll_Orderby_Amount_Asc_Sync();
            return entities;
        }

        //ExpenseCategoryId Async
        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_ExpenseCategoryId_Desc_Async()
        {
            var entities = await _repository.GetAll_Orderby_ExpenseCategoryId_Desc_Async();
            return entities;
        }

        public async Task<IEnumerable<ExpenseEntity>> GetAll_Orderby_ExpenseCategoryId_Asc_Async()
        {
            var entities = await _repository.GetAll_Orderby_ExpenseCategoryId_Asc_Async();
            return entities;
        }

        //ExpenseCategoryId Sync
        public IEnumerable<ExpenseEntity> GetAll_Orderby_ExpenseCategoryId_Desc_Sync()
        {
            var entities = _repository.GetAll_Orderby_ExpenseCategoryId_Desc_Sync();
            return entities;
        }

        public IEnumerable<ExpenseEntity> GetAll_Orderby_ExpenseCategoryId_Asc_Sync()
        {
            var entities = _repository.GetAll_Orderby_ExpenseCategoryId_Asc_Sync();
            return entities;
        }

        #endregion
    }
}
