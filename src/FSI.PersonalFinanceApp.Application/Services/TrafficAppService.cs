using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Application.Mapper;
using FSI.PersonalFinanceApp.Domain.Entities;
using FSI.PersonalFinanceApp.Domain.Interfaces;

namespace FSI.PersonalFinanceApp.Application.Services
{
    public class TrafficAppService : ITrafficAppService
    {
        private readonly ITrafficRepository _repository; // ✅ Declaração

        public TrafficAppService(ITrafficRepository repository) // ✅ Construtor
        {
            _repository = repository;
        }

        #region Methods from IBaseAppService

        public async Task<IEnumerable<TrafficDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(TrafficMapper.ToDto);
        }

        public IEnumerable<TrafficDto> GetAllSync()
        {
            var entities = _repository.GetAllSync();
            return entities.Select(TrafficMapper.ToDto);
        }

        public async Task<TrafficDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : TrafficMapper.ToDto(entity);
        }

        public TrafficDto? GetByIdSync(long id)
        {
            var entity = _repository.GetByIdSync(id);
            return entity is null ? null : TrafficMapper.ToDto(entity);
        }

        public async Task<long> AddAsync(TrafficDto dto)
        {
            var entity = TrafficMapper.ToEntity(dto);
            return await _repository.AddAsync(entity);
        }

        public long AddSync(TrafficDto dto)
        {
            var entity = TrafficMapper.ToEntity(dto);
            return _repository.AddSync(entity);
        }

        public async Task<bool> UpdateAsync(TrafficDto dto)
        {
            var entity = TrafficMapper.ToEntity(dto);
            return await _repository.UpdateAsync(entity);
        }

        public bool UpdateSync(TrafficDto dto)
        {
            var entity = TrafficMapper.ToEntity(dto);
            return _repository.UpdateSync(entity);
        }

        public async Task<bool> DeleteAsync(TrafficDto dto)
        {
            var entity = TrafficMapper.ToEntity(dto);
            return await _repository.DeleteAsync(entity);
        }

        public bool DeleteSync(TrafficDto dto)
        {
            var entity = TrafficMapper.ToEntity(dto);
            return _repository.DeleteSync(entity);
        }

        public async Task<IEnumerable<TrafficDto>> GetAllFilteredAsync(string filterBy, string value)
        {
            var entities = await _repository.GetAllFilteredAsync(filterBy, value);
            return entities.Select(TrafficMapper.ToDto);
        }

        public IEnumerable<TrafficDto> GetAllFilteredSync(string filterBy, string value)
        {
            var entities = _repository.GetAllFilteredSync(filterBy, value);
            return entities.Select(TrafficMapper.ToDto);
        }

        public async Task<IEnumerable<TrafficDto>> GetAllOrderedAsync(string orderBy, string direction)
        {
            var entities = await _repository.GetAllOrderedAsync(orderBy, direction);
            return entities.Select(TrafficMapper.ToDto);
        }

        public IEnumerable<TrafficDto> GetAllOrderedSync(string orderBy, string direction)
        {
            var entities = _repository.GetAllOrderedSync(orderBy, direction);
            return entities.Select(TrafficMapper.ToDto);
        }

        #endregion
    }
}
