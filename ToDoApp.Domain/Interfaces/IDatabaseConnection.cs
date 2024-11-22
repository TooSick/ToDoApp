using System.Data;

namespace ToDoApp.Domain.Interfaces
{
    public interface IDatabaseConnection
    {
        Task<IDbConnection> GetDatabaseConnectionAsync();
    }
}
