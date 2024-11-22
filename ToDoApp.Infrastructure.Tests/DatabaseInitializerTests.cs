using Microsoft.Data.SqlClient;
using ToDoApp.Infrastructure.Database;

namespace ToDoApp.Infrastructure.Tests
{
    public class DatabaseInitializerTests
    {
        private readonly DatabaseInitializer _databaseInitializer;

        public DatabaseInitializerTests()
        {
            _databaseInitializer = new DatabaseInitializer();
        }

        [Fact]
        public async Task InitializeAsync_ShouldCreateDatabaseIfNotExists()
        {
            string testMasterConnectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;";

            await _databaseInitializer.CreateDatabaseAsync(testMasterConnectionString);

            using var connection = new SqlConnection(testMasterConnectionString);
            await connection.OpenAsync();

            using var checkDbCommand = connection.CreateCommand();
            checkDbCommand.CommandText = "SELECT name FROM sys.databases WHERE name = 'ToDoApp'";
            var result = await checkDbCommand.ExecuteScalarAsync();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task InitializeAsync_ShouldCreateTableIfNotExists()
        {
            string testMasterConnectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;";
            string testDatabaseConnectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Database=ToDoApp;";

            await _databaseInitializer.CreateDatabaseAsync(testMasterConnectionString);
            await _databaseInitializer.CreateTableAsync(testDatabaseConnectionString);

            using var connection = new SqlConnection(testDatabaseConnectionString);
            await connection.OpenAsync();

            using var checkTableCommand = connection.CreateCommand();
            checkTableCommand.CommandText = @"
                SELECT 1 
                FROM sysobjects 
                WHERE name = 'ToDoItems' AND xtype = 'U'";
            var result = await checkTableCommand.ExecuteScalarAsync();

            Assert.NotNull(result);
        }
    }
}
