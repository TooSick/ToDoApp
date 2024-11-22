using ToDoApp.Domain.Enums;

namespace ToDoApp.API.ViewModels
{
    public class UpdateToDoItemViewModel
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
    }
}
