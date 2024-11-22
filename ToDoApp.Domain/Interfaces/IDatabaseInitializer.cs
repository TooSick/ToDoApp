namespace ToDoApp.Domain.Interfaces
{
    public interface IDatabaseInitializer
    {
        Task CreateDatabaseAsync(string connectionString);
        Task CreateTableAsync(string connectionString);
    }
}
