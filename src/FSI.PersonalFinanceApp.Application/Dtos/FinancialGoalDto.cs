namespace FSI.PersonalFinanceApp.Application.Dtos
{
    public class FinancialGoalDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime DueDate { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        public FinancialGoalDto()
        {
                
        }

        public FinancialGoalDto(long id)
        {
            Id = id;
        }
    }
}
