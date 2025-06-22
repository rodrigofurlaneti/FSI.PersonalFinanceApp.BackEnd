using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Application.Mapper;
using FSI.PersonalFinanceApp.Domain.Interfaces;

namespace FSI.PersonalFinanceApp.Application.Services
{
    public class TransactionAppService : ITransactionAppService
    {
        private readonly ITransactionRepository _repository;

        public TransactionAppService(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TransactionDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(TransactionMapper.ToDto);
        }

        public IEnumerable<TransactionDto> GetAllSync()
        {
            var entities = _repository.GetAllSync();
            return entities.Select(TransactionMapper.ToDto);
        }

        public async Task<TransactionDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : TransactionMapper.ToDto(entity);
        }

        public TransactionDto? GetByIdSync(long id)
        {
            var entity = _repository.GetByIdSync(id);
            return entity is null ? null : TransactionMapper.ToDto(entity);
        }

        public async Task<long> AddAsync(TransactionDto dto)
        {
            var entity = TransactionMapper.ToEntity(dto);
            return await _repository.AddAsync(entity);
        }

        public long AddSync(TransactionDto dto)
        {
            var entity = TransactionMapper.ToEntity(dto);
            return _repository.AddSync(entity);
        }

        public async Task UpdateAsync(TransactionDto dto)
        {
            var entity = TransactionMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }

        public void UpdateSync(TransactionDto dto)
        {
            var entity = TransactionMapper.ToEntity(dto);
            _repository.UpdateSync(entity);
        }

        public async Task DeleteAsync(TransactionDto dto)
        {
            var entity = TransactionMapper.ToEntity(dto);
            await _repository.DeleteAsync(entity);
        }

        public void DeleteSync(TransactionDto dto)
        {
            var entity = TransactionMapper.ToEntity(dto);
            _repository.DeleteSync(entity);
        }

        public async Task<IEnumerable<TransactionDto>> GetAllFilteredAsync(string filterBy, string value)
        {
            var entities = await _repository.GetAllFilteredAsync(filterBy, value);
            return entities.Select(TransactionMapper.ToDto);
        }

        public IEnumerable<TransactionDto> GetAllFilteredSync(string filterBy, string value)
        {
            var entities = _repository.GetAllFilteredSync(filterBy, value);
            return entities.Select(TransactionMapper.ToDto);
        }

        public async Task<IEnumerable<TransactionDto>> GetAllOrderedAsync(string orderBy, string direction)
        {
            var entities = await _repository.GetAllOrderedAsync(orderBy, direction);
            return entities.Select(TransactionMapper.ToDto);
        }

        public IEnumerable<TransactionDto> GetAllOrderedSync(string orderBy, string direction)
        {
            var entities = _repository.GetAllOrderedSync(orderBy, direction);
            return entities.Select(TransactionMapper.ToDto);
        }
    }
}
