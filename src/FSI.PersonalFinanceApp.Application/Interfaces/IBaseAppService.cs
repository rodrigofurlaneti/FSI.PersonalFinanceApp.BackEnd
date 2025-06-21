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
        Task AddAsync(TDto dto);
        void AddSync(TDto dto);

        // Update
        Task UpdateAsync(TDto dto);
        void UpdateSync(TDto dto);

        // Delete
        Task DeleteAsync(TDto dto); 
        void DeleteSync(TDto dto);

        //Ordered
        Task<IEnumerable<TDto>> GetAllOrderedAsync(string orderBy, string direction);
        IEnumerable<TDto> GetAllOrderedSync(string orderBy, string direction);

        //Filtered
        Task<IEnumerable<TDto>> GetAllFilteredAsync(string filterBy, string value);
        IEnumerable<TDto> GetAllFilteredSync(string filterBy, string value);

        #endregion
    }
}
