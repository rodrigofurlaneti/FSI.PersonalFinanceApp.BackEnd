namespace FSI.PersonalFinanceApp.Application.Messaging
{
    public class Envelope<T> : IMessageEnvelope<T>
    {
        public string Action { get; set; } = string.Empty;
        public T Payload { get; set; } = default!;
        public long MessagingId { get; set; }
    }
}
