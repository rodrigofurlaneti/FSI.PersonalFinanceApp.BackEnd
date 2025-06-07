using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Application.Mapper;
using FSI.PersonalFinanceApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.PersonalFinanceApp.Application.Services
{
    public class IncomeCategoryAppService : IIncomeCategoryAppService
    {
        private readonly IIncomeCategoryRepository _repository;

        public IncomeCategoryAppService(IIncomeCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<IncomeCategoryDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(IncomeCategoryMapper.ToDto);
        }

        public IEnumerable<IncomeCategoryDto> GetAllSync()
        {
            var entities = _repository.GetAllSync();
            return entities.Select(IncomeCategoryMapper.ToDto);
        }

        public async Task<IncomeCategoryDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : IncomeCategoryMapper.ToDto(entity);
        }

        public IncomeCategoryDto? GetByIdSync(long id)
        {
            var entity = _repository.GetByIdSync(id);
            return entity is null ? null : IncomeCategoryMapper.ToDto(entity);
        }

        public async Task AddAsync(IncomeCategoryDto dto)
        {
            var entity = IncomeCategoryMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
        }

        public void AddSync(IncomeCategoryDto dto)
        {
            var entity = IncomeCategoryMapper.ToEntity(dto);
            _repository.AddSync(entity);
        }

        public async Task UpdateAsync(IncomeCategoryDto dto)
        {
            var entity = IncomeCategoryMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }

        public void UpdateSync(IncomeCategoryDto dto)
        {
            var entity = IncomeCategoryMapper.ToEntity(dto);
            _repository.UpdateSync(entity);
        }

        public async Task DeleteAsync(IncomeCategoryDto dto)
        {
            var entity = IncomeCategoryMapper.ToEntity(dto);
            await _repository.DeleteAsync(entity);
        }
    }

}
