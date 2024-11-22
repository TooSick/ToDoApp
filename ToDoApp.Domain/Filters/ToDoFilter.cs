namespace ToDoApp.Domain.Filters
{
    public class ToDoFilter
    {
        public int? Status { get; set; }
        public int? Priority { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
