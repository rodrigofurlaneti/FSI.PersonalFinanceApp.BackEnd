namespace FSI.PersonalFinanceApp.Domain.Entities
{
    public class MessagingEntity : BaseEntity
    {
        public string Action { get; set; } = string.Empty;
        public string QueueName { get; set; } = string.Empty;
        public string MessageContent { get; set; } = string.Empty;
        public bool IsProcessed { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;

        public MessagingEntity() { }

        public MessagingEntity(string action, string queueName, string messageContent, bool isProcessed, string errorMessage)
        {
            Action = action;
            QueueName = queueName;
            MessageContent = messageContent;
            IsProcessed = isProcessed;
            ErrorMessage = errorMessage;
        }
    }
}
