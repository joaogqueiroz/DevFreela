using DevFreela.Core.IntegrationEvents;
using DevFreela.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevFreela.Application.Consumers
{
    public class PaymentApprovedConsumer : BackgroundService
    {
        private const string PAYMENT_APPROVED_QUEUE = "PaymentApproved";
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public PaymentApprovedConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var factory = new ConnectionFactory
            {
                HostName = "localhost",

            };
            _connection = factory.CreateConnection();
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

                var paymentsApprovedIntegrationEvent = JsonSerializer.Deserialize<PaymentApprovedIntegrationEvent>(paymentsApprovedJson);
                await FinishProject(paymentsApprovedIntegrationEvent.IdProject);

                _channel.BasicAck(eventArgs.DeliveryTag, false);

            };

            _channel.BasicConsume(PAYMENT_APPROVED_QUEUE, false, consumer);
            return Task.CompletedTask;
        }
        public async Task FinishProject(int Id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var projectRepository = scope.ServiceProvider.GetRequiredService<IProjectRepository>();

                var project = await projectRepository.GetByIdAsync(Id);

                project.Finish();

                await projectRepository.SaveChangesAsync();
            }
        }
    }
}
