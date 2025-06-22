using FSI.PersonalFinanceApp.Application.Dtos;

namespace FSI.PersonalFinanceApp.Application.Interfaces
{
    public interface IBaseAppService<TDto>
    {
        #region Métodos padrão das entidades

        // Get All
        Task<IEnumerable<TDto>> GetAllAsync();
        IEnumerable<TDto> GetAllSync();

        // Get By Id
        Task<TDto?> GetByIdAsync(long id);
        TDto? GetByIdSync(long id);

        // Add
        Task<long> AddAsync(TDto dto);
        long AddSync(TDto dto);

        // Update
        Task<bool> UpdateAsync(TDto dto);
        bool UpdateSync(TDto dto);

        // Delete
        Task<bool> DeleteAsync(TDto dto);
        bool DeleteSync(TDto dto);

        //Ordered
        Task<IEnumerable<TDto>> GetAllOrderedAsync(string orderBy, string direction);
        IEnumerable<TDto> GetAllOrderedSync(string orderBy, string direction);

        //Filtered
        Task<IEnumerable<TDto>> GetAllFilteredAsync(string filterBy, string value);
        IEnumerable<TDto> GetAllFilteredSync(string filterBy, string value);

        #endregion
    }
}
