namespace FSI.PersonalFinanceApp.Domain.Entities
{
    public class IncomeEntity : BaseEntity
    {
        public decimal Amount { get; set; }
        public DateTime IncomeDate { get; set; }
        public string? Description { get; set; }
        public DateTime? ReceivedAt { get; set; }

        public long IncomeCategoryId { get; set; }

        // Navegação opcional
        public IncomeCategoryEntity? IncomeCategory { get; set; }

        public void MarkAsReceived(DateTime receivedDate)
        {
            ReceivedAt = receivedDate;
            UpdateTimestamp();
        }
    }
}
