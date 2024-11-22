namespace ToDoApp.Domain.Interfaces
{
    public interface IService<T, F>
    {
        Task<IEnumerable<T>> GetAllAsync(F filter);
        Task<T?> GetByIdAsync(int id);
        Task<int> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
