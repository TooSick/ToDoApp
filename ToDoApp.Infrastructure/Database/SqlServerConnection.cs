using Microsoft.Data.SqlClient;
using System.Data;
using ToDoApp.Domain.Interfaces;

namespace ToDoApp.Infrastructure.Database
{
    public class SqlServerConnection : IDatabaseConnection, IDisposable
    {
        private readonly SqlConnection _connection;


        public SqlServerConnection(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }


        public async Task<IDbConnection> GetDatabaseConnectionAsync()
        {
            if (_connection.State != ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            return _connection;
        }

        public void Dispose()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
            _connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
