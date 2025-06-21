using System.ComponentModel.DataAnnotations;

namespace FSI.PersonalFinanceApp.Application.Dtos
{
    public class ExpenseDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name can't exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Due date is required")]
        public DateTime DueDate { get; set; }

        [StringLength(300, ErrorMessage = "Description can't exceed 300 characters")]
        public string? Description { get; set; }

        public DateTime? PaidAt { get; set; }

        [Required(ErrorMessage = "Expense category is required")]
        [Range(1, long.MaxValue, ErrorMessage = "Expense category must be a valid value")]
        public long ExpenseCategoryId { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        public ExpenseDto() { }

        public ExpenseDto(long id)
        {
            Id = id;
        }
    }
}
