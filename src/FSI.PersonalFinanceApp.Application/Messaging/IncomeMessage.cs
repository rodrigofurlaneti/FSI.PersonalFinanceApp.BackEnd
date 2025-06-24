using FSI.PersonalFinanceApp.Application.Dtos;

namespace FSI.PersonalFinanceApp.Application.Messaging
{
    public class IncomeMessage
    {
        public string Action { get; set; } = string.Empty; // "Create", "Update", "Delete"
        public IncomeDto Payload { get; set; } = new();
        public long MessagingId { get; set; } // NOVO: Id da mensagem na tabela Messaging
    }
}
