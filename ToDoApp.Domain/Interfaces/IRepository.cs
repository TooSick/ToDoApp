namespace ToDoApp.Domain.Interfaces
{
    public interface IRepository<T, F>
    {
        Task<IEnumerable<T>> GetAllAsync(F filter);
        Task<T?> GetByIdAsync(int id);
        Task<int> CreateAsync(T toDoItem);
        Task UpdateAsync(T toDoItem);
        Task DeleteAsync(int id);
    }
}
