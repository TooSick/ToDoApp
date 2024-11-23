using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.AsyncDataServices
{
    public class RabbitMqClient : IMessageBusClient<ToDoItem>
    {
        private readonly IConnectionFactory _connectionFactory;

        public RabbitMqClient()
        {
            _connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
            };
        }

        public async Task PublishAsync(ToDoItem message)
        {
            try
            {
                var channel = await GetChannelAsync();
                string serializedMessage = JsonSerializer.Serialize<ToDoItem>(message);
                var body = Encoding.UTF8.GetBytes(serializedMessage);

                await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "ToDoItems", body: body);
            }
            catch { }
        }

        private async Task<IChannel> GetChannelAsync()
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "ToDoItems", durable: false, exclusive: false, autoDelete: false,
                arguments: null);

            return channel;
        }
    }
}
