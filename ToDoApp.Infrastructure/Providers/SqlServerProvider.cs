using System.Text;
using ToDoApp.Domain.Filters;
using ToDoApp.Domain.Interfaces;

namespace ToDoApp.Infrastructure.Providers
{
    public class SqlServerProvider : ISqlProvider
    {
        public string Create() => @"
            INSERT INTO ToDoItems (Title, Description, DueDate, Priority, Status)
            OUTPUT INSERTED.Id
            VALUES (@Title, @Description, @DueDate, @Priority, @Status);
        ";

        public string Delete() => @"
            DELETE FROM ToDoItems
            WHERE Id = @Id;
        ";

        public string GetAll(ToDoFilter filter)
        {
            StringBuilder queryBuilder = new StringBuilder(50);

            queryBuilder.Append("SELECT Id, Title, Description, DueDate, Priority, Status FROM ToDoItems WHERE 1=1");

            if (filter.Status.HasValue)
                queryBuilder.Append(" AND Status = @Status");
            if (filter.Priority.HasValue)
                queryBuilder.Append(" AND Priority = @Priority");
            if (filter.DueDate.HasValue)
                queryBuilder.Append(" AND DueDate = @DueDate");

            return queryBuilder.ToString();
        }

        public string Update() => @"
            UPDATE ToDoItems
            SET 
                Title = @Title,
                Description = @Description,
                DueDate = @DueDate,
                Priority = @Priority,
                Status = @Status
            WHERE Id = @Id;
        ";

        public string GetById() => @"
            SELECT Id, Title, Description, DueDate, Priority, Status
            FROM ToDoItems
            WHERE Id = @Id;
        ";
    }
}
