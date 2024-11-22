namespace ToDoApp.API.ViewModels
{
    public class ToDoItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public string Priority { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
