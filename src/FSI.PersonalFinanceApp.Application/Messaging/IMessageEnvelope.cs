namespace FSI.PersonalFinanceApp.Application.Messaging
{
    public interface IMessageEnvelope<T>
    {
        string Action { get; set; }
        T Payload { get; set; }
        long MessagingId { get; set; }
    }
}
