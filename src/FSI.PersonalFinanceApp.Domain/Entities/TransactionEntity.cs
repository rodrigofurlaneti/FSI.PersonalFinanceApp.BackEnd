namespace FSI.PersonalFinanceApp.Domain.Entities
{
    public class TransactionEntity : BaseEntity
    {
        public long? AccountFromId { get; set; }
        public AccountEntity? AccountFrom { get; set; }

        public long? AccountToId { get; set; }
        public AccountEntity? AccountTo { get; set; }

        public long? ExpenseId { get; set; }
        public ExpenseEntity? Expense { get; set; }

        public long? IncomeId { get; set; }
        public IncomeEntity? Income { get; set; }

        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}
