namespace FSI.PersonalFinanceApp.Application.Dtos
{
    public class ExpenseDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string? Description { get; set; }
        public DateTime? PaidAt { get; set; }
        public long ExpenseCategoryId { get; set; }
        public DateTime? CreatedAt { get; set; } // para manter rastreamento, opcional
        public DateTime? UpdatedAt { get; set; } // para rastreamento
        public bool IsActive { get; set; } = true;

        public ExpenseDto()
        {
                
        }

        public ExpenseDto(long id)
        {
            Id = id;
        }
    }
}
