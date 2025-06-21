namespace FSI.PersonalFinanceApp.Domain.Entities
{
    public class ExpenseEntity : BaseEntity
    {
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string? Description { get; set; }
        public DateTime? PaidAt { get; set; }

        public long ExpenseCategoryId { get; set; }

        public ExpenseCategoryEntity? ExpenseCategory { get; set; }

        public void MarkAsPaid(DateTime paymentDate)
        {
            PaidAt = paymentDate;
            UpdateTimestamp();
        }
    }
}
