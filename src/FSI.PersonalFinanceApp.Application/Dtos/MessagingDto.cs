namespace FSI.PersonalFinanceApp.Application.Dtos
{
    public class MessagingDto
    {
        public long Id { get; set; }
        public string Action { get; set; } = string.Empty;
        public string QueueName { get; set; } = string.Empty;
        public string MessageContent { get; set; } = string.Empty;
        public bool IsProcessed { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        public MessagingDto()
        {

        }

        public MessagingDto(long id)
        {
            Id = id;
        }

        public MessagingDto(string action, string queueName, string messageContent, bool isProcessed, string errorMessage)
        {
            Action = action;
            QueueName = queueName;
            MessageContent = messageContent;
            IsProcessed = isProcessed;
            ErrorMessage = errorMessage;
        }
    }
}
