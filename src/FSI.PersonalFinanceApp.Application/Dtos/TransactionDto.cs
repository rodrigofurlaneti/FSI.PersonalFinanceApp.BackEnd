namespace FSI.PersonalFinanceApp.Application.Dtos
{
    public class TransactionDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public long? AccountFromId { get; set; }
        public long? AccountToId { get; set; }
        public long? ExpenseId { get; set; }
        public long? IncomeId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        public TransactionDto()
        {
                
        }

        public TransactionDto(long id)
        {
            Id = id;
        }

    }
}
