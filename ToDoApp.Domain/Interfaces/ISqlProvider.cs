using ToDoApp.Domain.Filters;

namespace ToDoApp.Domain.Interfaces
{
    public interface ISqlProvider
    {
        string GetAll(ToDoFilter filter);
        string Create();
        string Update();
        string Delete();
        string GetById();
    }
}
