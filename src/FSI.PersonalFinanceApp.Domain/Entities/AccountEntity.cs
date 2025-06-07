namespace FSI.PersonalFinanceApp.Domain.Entities
{
    public class AccountEntity : BaseEntity
    {
        public decimal Balance { get; private set; } = 0;

        public string? Description { get; set; }

        public ICollection<TransactionEntity>? TransactionsFrom { get; set; }
        public ICollection<TransactionEntity>? TransactionsTo { get; set; }

        public void Deposit(decimal amount)
        {
            Balance += amount;
            UpdateTimestamp();
        }

        public void Withdraw(decimal amount)
        {
            if (Balance < amount)
                throw new InvalidOperationException("Insufficient funds.");

            Balance -= amount;
            UpdateTimestamp();
        }
    }
}
