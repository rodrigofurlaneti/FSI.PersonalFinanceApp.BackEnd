using FSI.PersonalFinanceApp.Domain.Entities;

namespace FSI.PersonalFinanceApp.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        #region Entity default methods

        // Get All
        Task<IEnumerable<T>> GetAllAsync();
        IEnumerable<T> GetAllSync();

        // Get By Id
        Task<T?> GetByIdAsync(long id);
        T? GetByIdSync(long id);

        // Add
        Task<long> AddAsync(T entity);
        long AddSync(T entity);

        // Update
        Task<bool> UpdateAsync(T entity);
        bool UpdateSync(T entity);

        //Delete
        Task<bool> DeleteAsync(T entity);
        bool DeleteSync(T entity);

        // Ordered
        Task<IEnumerable<T>> GetAllOrderedAsync(string orderBy, string direction);
        IEnumerable<T> GetAllOrderedSync(string orderBy, string direction);

        // Filtered
        Task<IEnumerable<T>> GetAllFilteredAsync(string filterBy, string value);
        IEnumerable<T> GetAllFilteredSync(string filterBy, string value);

        #endregion
    }
}
