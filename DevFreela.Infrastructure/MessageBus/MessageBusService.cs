using DevFreela.Core.Services;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
namespace DevFreela.Infrastructure.MessageBus
{
  public class MessageBusService : IMessageBusService
  {
    private readonly ConnectionFactory _factory;

    public MessageBusService()
    {
      _factory = new ConnectionFactory
      {
        HostName = "localHost"
      };
    }
    public void Publish(string queue, byte[] message)
    {
      using (var connection = _factory.CreateConnection())
      {
        using (var channel = connection.CreateModel())
        {
          //garantir que a fila esteja criada
          channel.QueueDeclare(
            queue: queue,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
          channel.BasicPublish(
            exchange: "",
            routingKey: queue,
            basicProperties: null,
            body: message
            );
          //publicar mensagem
        }
      }
    }
  }
}