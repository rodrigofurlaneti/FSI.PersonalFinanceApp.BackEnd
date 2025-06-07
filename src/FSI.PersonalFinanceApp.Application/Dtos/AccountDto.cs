namespace FSI.PersonalFinanceApp.Application.Dtos
{
    public class AccountDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Balance { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
