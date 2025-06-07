namespace FSI.PersonalFinanceApp.Application.Dtos
{
    public class ExpenseCategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        public ExpenseCategoryDto()
        {
                
        }

        public ExpenseCategoryDto(long id)
        {
            Id = id;
        }
    }
}
