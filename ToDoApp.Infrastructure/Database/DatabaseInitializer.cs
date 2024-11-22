using Microsoft.Data.SqlClient;
using ToDoApp.Domain.Interfaces;

namespace ToDoApp.Infrastructure.Database
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        public async Task CreateDatabaseAsync(string connectionString)
        {
            string databaseName = "ToDoApp";

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using var checkDbCommand = connection.CreateCommand();
                checkDbCommand.CommandText = $@"
                    IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = @DatabaseName)
                    BEGIN
                        CREATE DATABASE [{databaseName}]
                    END";
                checkDbCommand.Parameters.AddWithValue("@DatabaseName", databaseName);

                await checkDbCommand.ExecuteNonQueryAsync();
                await connection.CloseAsync();
            }
        }

        public async Task CreateTableAsync(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using var createTableCommand = connection.CreateCommand();
                createTableCommand.CommandText = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'ToDoItems' AND xtype = 'U')
                    BEGIN
                        CREATE TABLE ToDoItems (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Title NVARCHAR(100) NOT NULL,
                            Description NVARCHAR(1000),
                            DueDate DATETIME NOT NULL,
                            Priority INT NOT NULL,
                            Status INT NOT NULL
                        )
                    END";
                await createTableCommand.ExecuteNonQueryAsync();
                await connection.CloseAsync();
            }

        }
    }
}
