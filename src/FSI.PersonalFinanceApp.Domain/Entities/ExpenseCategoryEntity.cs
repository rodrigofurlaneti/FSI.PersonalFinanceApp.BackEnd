namespace FSI.PersonalFinanceApp.Domain.Entities
{
    public class ExpenseCategoryEntity : BaseEntity
    {
        public ICollection<ExpenseEntity>? Expenses { get; set; }
    }
}
