namespace FSI.PersonalFinanceApp.Application.Dtos
{
    public class IncomeDto : IBaseDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime IncomeDate { get; set; }
        public string? Description { get; set; }
        public DateTime? ReceivedAt { get; set; }
        public long IncomeCategoryId { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        public IncomeDto() { }
        public IncomeDto(long id) { Id = id; }

    }
}
