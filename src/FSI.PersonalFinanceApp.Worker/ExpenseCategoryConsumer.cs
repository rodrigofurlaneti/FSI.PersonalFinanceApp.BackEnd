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
            string nameQueue = "expense-category-queue";

            var factory = new ConnectionFactory
            {
                HostName = _config["RabbitMq:Host"] ?? "localhost",
                UserName = _config["RabbitMq:User"] ?? "guest",
                Password = _config["RabbitMq:Password"] ?? "guest"
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: nameQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (sender, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine("📥 Message received from RabbitMQ:");
                    Console.WriteLine(message);

                    var envelope = JsonSerializer.Deserialize<ExpenseCategoryMessage>(
                        message,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    if (envelope == null)
                    {
                        Console.WriteLine("❌ Envelope is null. Check message format.");
                        return;
                    }

                    Console.WriteLine($"✔ Action received: {envelope.Action}");

                    Console.WriteLine($"✔ Payload: {JsonSerializer.Serialize(envelope.Payload)}");

                    using var scope = _scopeFactory.CreateScope();
                    
                    var service = scope.ServiceProvider.GetRequiredService<IExpenseCategoryAppService>();
                    
                    var messagingService = scope.ServiceProvider.GetRequiredService<IMessagingAppService>();

                    long? createdId = null;

                    switch (envelope.Action.ToLowerInvariant())
                    {
                        case "create":
                            createdId = await service.AddAsync(envelope.Payload); 
                            break;
                        case "update":
                            await service.UpdateAsync(envelope.Payload);
                            break;
                        case "delete":
                            await service.DeleteAsync(envelope.Payload);
                            break;
                        default:
                            Console.WriteLine($"⚠ Action not recognized: {envelope.Action}");
                            break;
                    }

                    // ✅ Updates the processing status of the record in the database to processed
                    if (envelope.MessagingId > 0 && envelope.Action.Equals("create", StringComparison.OrdinalIgnoreCase))
                    {
                        await UpdateProcessedMessageAsync(messagingService, envelope, nameQueue, createdId);
                    }

                    // ✅ Manual confirmation that the message was processed successfully
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    // ❌ Não dar o Ack -> mensagem permanece na fila
                    // 🔴 Logar erro de parsing ou de serviço
                    Console.WriteLine($"Error processing message: {ex.Message}");
                }
            };

            channel.BasicConsume(queue: nameQueue, autoAck: false, consumer: consumer);

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

        private async Task UpdateProcessedMessageAsync(IMessagingAppService messagingService,ExpenseCategoryMessage envelope,
            string queueName,long? createdId)
        {
            if (createdId != null)
            {
                envelope.Payload.Id = createdId.Value;
                envelope.Payload.UpdatedAt = DateTime.Now;

                var updatedContent = JsonSerializer.Serialize(envelope);

                await messagingService.UpdateAsync(new MessagingDto
                {
                    Id = envelope.MessagingId,
                    Action = "Create",
                    QueueName = queueName,
                    MessageContent = updatedContent,
                    IsProcessed = true,
                    ErrorMessage = string.Empty,
                    UpdatedAt = DateTime.Now,
                    IsActive = true
                });

                Console.WriteLine($"✔ Message ID {envelope.MessagingId} marked as processed.");
            }
            else
            {
                await messagingService.UpdateAsync(new MessagingDto
                {
                    Id = envelope.MessagingId,
                    Action = envelope.Action,
                    QueueName = queueName,
                    MessageContent = JsonSerializer.Serialize(envelope),
                    IsProcessed = false,
                    ErrorMessage = "Failed to insert expense category into database.",
                    UpdatedAt = DateTime.Now,
                    IsActive = false
                });

                Console.WriteLine($"❌ Failed to process message ID {envelope.MessagingId}: creation returned null.");
            }
        }
    }
}
