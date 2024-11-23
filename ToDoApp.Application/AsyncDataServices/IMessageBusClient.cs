namespace ToDoApp.Application.AsyncDataServices
{
    public interface IMessageBusClient<T>
    {
        Task PublishAsync(T message);
    }
}
