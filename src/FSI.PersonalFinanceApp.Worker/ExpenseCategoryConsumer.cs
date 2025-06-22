using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;

namespace FSI.PersonalFinanceApp.Worker
{
    public class ExpenseCategoryConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _config;

        public ExpenseCategoryConsumer(IServiceScopeFactory scopeFactory, IConfiguration config)
        {
            _scopeFactory = scopeFactory;
            _config = config;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _config["RabbitMq:Host"] ?? "localhost",
                UserName = _config["RabbitMq:User"] ?? "guest",
                Password = _config["RabbitMq:Password"] ?? "guest"
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "expense-category-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (sender, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var envelope = JsonSerializer.Deserialize<ExpenseCategoryEnvelope>(message);

                    if (envelope is null) return;

                    using var scope = _scopeFactory.CreateScope();
                    var service = scope.ServiceProvider.GetRequiredService<IExpenseCategoryAppService>();

                    switch (envelope.Action.ToLowerInvariant())
                    {
                        case "create":
                            await service.AddAsync(envelope.Payload);
                            break;
                        case "update":
                            await service.UpdateAsync(envelope.Payload);
                            break;
                        case "delete":
                            await service.DeleteAsync(envelope.Payload);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // 🔴 Logar erro de parsing ou de serviço
                    Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
                }
            };

            channel.BasicConsume(queue: "expense-category-queue", autoAck: true, consumer: consumer);

            // 🔄 Loop para manter o serviço ativo enquanto não for cancelado
            return Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }

                // Cleanup opcional:
                channel.Close();
                connection.Close();
            }, stoppingToken);
        }
    }

    public class ExpenseCategoryEnvelope
    {
        public string Action { get; set; } = "";
        public ExpenseCategoryDto Payload { get; set; } = new();
    }
}
