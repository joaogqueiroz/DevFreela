using DevFreela.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.Application.Consumers
{
    public class PaymentApprovedConsumer : BackgroundService
    {
        private const string PAYMENT_APPROVED_QUEUE = "PaymentApproved";
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PaymentApprovedConsumer> _logger;

        public PaymentApprovedConsumer(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<PaymentApprovedConsumer> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            var rabbitMq = configuration.GetSection("RabbitMQ");
            var _factory = new ConnectionFactory
            {
                HostName = rabbitMq["HostName"],
                Port = int.Parse(rabbitMq["Port"]),
                UserName = rabbitMq["UserName"],
                Password = rabbitMq["Password"]
            };

            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: PAYMENT_APPROVED_QUEUE,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );


        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, eventArgs) =>
            {
                var paymentsApprovedBytes = eventArgs.Body.ToArray();
                var paymentsApprovedJson = Encoding.UTF8.GetString(paymentsApprovedBytes);

                // This handler is async void: an exception escaping it would stop the whole API
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var handler = new PaymentApprovedMessageHandler(scope.ServiceProvider.GetRequiredService<IProjectRepository>());

                    if (!await handler.HandleAsync(paymentsApprovedJson))
                    {
                        _logger.LogWarning("Payment-approved message dropped: unreadable or unknown project");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Payment-approved message could not be processed");
                }

                // Requeueing would redeliver the same message forever, so it is always removed
                _channel.BasicAck(eventArgs.DeliveryTag, false);

            };

            _channel.BasicConsume(PAYMENT_APPROVED_QUEUE, false, consumer);
            return Task.CompletedTask;
        }
    }
}
