namespace FSI.PersonalFinanceApp.Application.Messaging
{
    public interface IMessageQueuePublisher
    {
        void Publish<T>(T message, string queueName);
    }
}
