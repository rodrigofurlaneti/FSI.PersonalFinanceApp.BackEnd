using FSI.PersonalFinanceApp.Application.Dtos;
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Application.Messaging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

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

                    Console.WriteLine("📥 Mensagem recebida do RabbitMQ:");
                    Console.WriteLine(message);

                    var envelope = JsonSerializer.Deserialize<ExpenseCategoryMessage>(
                        message,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    if (envelope == null)
                    {
                        Console.WriteLine("❌ Envelope está nulo. Verifique o formato da mensagem.");
                        return;
                    }

                    Console.WriteLine($"✔ Ação recebida: {envelope.Action}");

                    Console.WriteLine($"✔ Payload: {JsonSerializer.Serialize(envelope.Payload)}");

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
                        default:
                            Console.WriteLine($"⚠ Ação não reconhecida: {envelope.Action}");
                            break;
                    }

                    // ✅ Confirmação manual de que a mensagem foi processada com sucesso
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    // ❌ Não dar o Ack -> mensagem permanece na fila
                    // 🔴 Logar erro de parsing ou de serviço
                    Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
                }
            };

            channel.BasicConsume(queue: "expense-category-queue", autoAck: false, consumer: consumer);

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
}
