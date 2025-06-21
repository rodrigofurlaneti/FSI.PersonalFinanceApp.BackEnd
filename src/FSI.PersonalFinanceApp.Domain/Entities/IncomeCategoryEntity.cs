namespace FSI.PersonalFinanceApp.Domain.Entities
{
    public class IncomeCategoryEntity : BaseEntity
    {
        public ICollection<IncomeEntity>? Incomes { get; set; }
    }
}
