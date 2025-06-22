using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Application.Mapper;
using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;

namespace FSI.PersonalFinanceApp.Application.Services
{
    public class MessagingAppService : IMessagingAppService
    {
        private readonly IMessagingRepository _repository; // ✅ Declaração

        public MessagingAppService(IMessagingRepository repository) // ✅ Construtor
        {
            _repository = repository;
        }

        #region Methods from IBaseAppService

        public async Task<IEnumerable<MessagingDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(MessagingMapper.ToDto);
        }

        public IEnumerable<MessagingDto> GetAllSync()
        {
            var entities = _repository.GetAllSync();
            return entities.Select(MessagingMapper.ToDto);
        }

        public async Task<MessagingDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : MessagingMapper.ToDto(entity);
        }

        public MessagingDto? GetByIdSync(long id)
        {
            var entity = _repository.GetByIdSync(id);
            return entity is null ? null : MessagingMapper.ToDto(entity);
        }

        public async Task AddAsync(MessagingDto dto)
        {
            var entity = MessagingMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
        }

        public void AddSync(MessagingDto dto)
        {
            var entity = MessagingMapper.ToEntity(dto);
            _repository.AddSync(entity);
        }

        public async Task UpdateAsync(MessagingDto dto)
        {
            var entity = MessagingMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }

        public void UpdateSync(MessagingDto dto)
        {
            var entity = MessagingMapper.ToEntity(dto);
            _repository.UpdateSync(entity);
        }

        public async Task DeleteAsync(MessagingDto dto)
        {
            var entity = MessagingMapper.ToEntity(dto);
            await _repository.DeleteAsync(entity);
        }

        public void DeleteSync(MessagingDto dto)
        {
            var entity = MessagingMapper.ToEntity(dto);
            _repository.DeleteSync(entity);
        }

        public async Task<IEnumerable<MessagingDto>> GetAllFilteredAsync(string filterBy, string value)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MessagingDto> GetAllFilteredSync(string filterBy, string value)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MessagingDto>> GetAllOrderedAsync(string orderBy, string direction)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MessagingDto> GetAllOrderedSync(string orderBy, string direction)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Additional Methods

        #endregion
    }
}
