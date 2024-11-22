using Microsoft.Data.SqlClient;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;
using ToDoApp.Domain.Filters;
using ToDoApp.Domain.Interfaces;

namespace ToDoApp.Infrastructure.Repositories
{
    public class ToDoRepository : IRepository<ToDoItem, ToDoFilter>
    {
        private readonly IDatabaseConnection _dbConnection;
        private readonly ISqlProvider _sqlProvider;


        public ToDoRepository(IDatabaseConnection dbConnection, ISqlProvider sqlProvider)
        {
            _dbConnection = dbConnection;
            _sqlProvider = sqlProvider;
        }


        public async Task<int> CreateAsync(ToDoItem toDoItem)
        {
            var query = _sqlProvider.Create();
            var connection = await _dbConnection.GetDatabaseConnectionAsync();

            var command = (SqlCommand)connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@Title", toDoItem.Title);
            command.Parameters.AddWithValue("@Description", toDoItem.Description);
            command.Parameters.AddWithValue("@DueDate", toDoItem.DueDate);
            command.Parameters.AddWithValue("@Priority", (int)toDoItem.Priority);
            command.Parameters.AddWithValue("@Status", (int)toDoItem.Status);

            return (int)await command.ExecuteScalarAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var query = _sqlProvider.Delete();
            var connection = await _dbConnection.GetDatabaseConnectionAsync();

            var command = (SqlCommand)connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("id", id);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<ToDoItem>> GetAllAsync(ToDoFilter filter)
        {
            var query = _sqlProvider.GetAll(filter);
            var connection = await _dbConnection.GetDatabaseConnectionAsync();

            var command = (SqlCommand)connection.CreateCommand();
            command.CommandText = query;

            if (filter.Status.HasValue)
                command.Parameters.AddWithValue("@Status", filter.Status);
            if (filter.Priority.HasValue)
                command.Parameters.AddWithValue("@Priority", filter.Priority);
            if (filter.DueDate.HasValue)
                command.Parameters.AddWithValue("@DueDate", filter.DueDate);

            var toDoItems = new List<ToDoItem>();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var toDoItem = new ToDoItem
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                        ? string.Empty
                        : reader.GetString(reader.GetOrdinal("Description")),
                    DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                    Priority = (Priority)reader.GetInt32(reader.GetOrdinal("Priority")),
                    Status = (Status)reader.GetInt32(reader.GetOrdinal("Status"))
                };

                toDoItems.Add(toDoItem);
            }

            return toDoItems;
        }

        public async Task<ToDoItem?> GetByIdAsync(int id)
        {
            var query = _sqlProvider.GetById();
            var connection = await _dbConnection.GetDatabaseConnectionAsync();

            var command = (SqlCommand)connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("id", id);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new ToDoItem
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                        ? string.Empty
                        : reader.GetString(reader.GetOrdinal("Description")),
                    DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                    Priority = (Priority)reader.GetInt32(reader.GetOrdinal("Priority")),
                    Status = (Status)reader.GetInt32(reader.GetOrdinal("Status"))
                };
            }

            return null;
        }

        public async Task UpdateAsync(ToDoItem toDoItem)
        {
            var query = _sqlProvider.Update();
            var connection = await _dbConnection.GetDatabaseConnectionAsync();

            var command = (SqlCommand)connection.CreateCommand();
            command.CommandText = query;

            command.Parameters.AddWithValue("@Title", toDoItem.Title);
            command.Parameters.AddWithValue("@Description", toDoItem.Description);
            command.Parameters.AddWithValue("@DueDate", toDoItem.DueDate);
            command.Parameters.AddWithValue("@Priority", (int)toDoItem.Priority);
            command.Parameters.AddWithValue("@Status", (int)toDoItem.Status);
            command.Parameters.AddWithValue("@id", toDoItem.Id);

            await command.ExecuteNonQueryAsync();
        }
    }
}
