using ToDoApp.Domain.Filters;
using ToDoApp.Infrastructure.Providers;

namespace ToDoApp.Infrastructure.Tests
{
    public class SqlServerProviderTests
    {
        private readonly SqlServerProvider _provider;

        public SqlServerProviderTests()
        {
            _provider = new SqlServerProvider();
        }

        [Fact]
        public void Create_ShouldReturnCorrectQuery()
        {
            var query = _provider.Create().Replace("\n", "").Replace("\r", "").Replace(" ", "");

            var expectedQuery = @"
                INSERT INTO ToDoItems (Title, Description, DueDate, Priority, Status)
                OUTPUT INSERTED.Id
                VALUES (@Title, @Description, @DueDate, @Priority, @Status);
            ".Replace("\n", "").Replace("\r", "").Replace(" ", "");
            Assert.Equal(expectedQuery, query);
        }

        [Fact]
        public void Delete_ShouldReturnCorrectQuery()
        {
            var query = _provider.Delete().Replace("\n", "").Replace("\r", "").Replace(" ", "");

            var expectedQuery = @"
                DELETE FROM ToDoItems
                WHERE Id = @Id;
            ".Replace("\n", "").Replace("\r", "").Replace(" ", "");
            Assert.Equal(expectedQuery, query);
        }

        [Fact]
        public void GetAll_ShouldReturnCorrectQuery_WithNoFilters()
        {
            var filter = new ToDoFilter();

            var query = _provider.GetAll(filter).Replace("\n", "").Replace("\r", "").Replace(" ", "");

            var expectedQuery = @"
                SELECT Id, Title, Description, DueDate, Priority, Status FROM ToDoItems WHERE 1=1
            ".Replace("\n", "").Replace("\r", "").Replace(" ", "");
            Assert.Equal(expectedQuery, query);
        }

        [Fact]
        public void GetAll_ShouldReturnCorrectQuery_WithFilters()
        {
            var filter = new ToDoFilter
            {
                Status = 1,
                Priority = 2,
                DueDate = DateTime.UtcNow
            };

            var query = _provider.GetAll(filter).Replace("\n", "").Replace("\r", "").Replace(" ", "");

            var expectedQuery = @"
                SELECT Id, Title, Description, DueDate, Priority, Status FROM ToDoItems WHERE 1=1
                AND Status = @Status
                AND Priority = @Priority
                AND DueDate = @DueDate
            ".Replace("\n", "").Replace("\r", "").Replace(" ", "");
            Assert.Equal(expectedQuery, query);
        }

        [Fact]
        public void Update_ShouldReturnCorrectQuery()
        {
            var query = _provider.Update().Replace("\n", "").Replace("\r", "").Replace(" ", "");

            var expectedQuery = @"
                UPDATE ToDoItems
                SET 
                    Title = @Title,
                    Description = @Description,
                    DueDate = @DueDate,
                    Priority = @Priority,
                    Status = @Status
                WHERE Id = @Id;
            ".Replace("\n", "").Replace("\r", "").Replace(" ", "");
            Assert.Equal(expectedQuery, query);
        }

        [Fact]
        public void GetById_ShouldReturnCorrectQuery()
        {
            var query = _provider.GetById().Replace("\n", "").Replace("\r", "").Replace(" ", "");

            var expectedQuery = @"
                SELECT Id, Title, Description, DueDate, Priority, Status
                FROM ToDoItems
                WHERE Id = @Id;
            ".Replace("\n", "").Replace("\r", "").Replace(" ", "");
            Assert.Equal(expectedQuery, query);
        }
    }
}
