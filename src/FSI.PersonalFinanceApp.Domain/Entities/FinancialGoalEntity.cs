namespace FSI.PersonalFinanceApp.Domain.Entities
{
    public class FinancialGoalEntity : BaseEntity
    {
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; } = 0;
        public DateTime DueDate { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; } = false;

        public void MarkAsCompleted()
        {
            IsCompleted = true;
            UpdateTimestamp();
        }

        public void UpdateProgress(decimal amount)
        {
            CurrentAmount += amount;
            if (CurrentAmount >= TargetAmount)
                MarkAsCompleted();

            UpdateTimestamp();
        }
    }
}
