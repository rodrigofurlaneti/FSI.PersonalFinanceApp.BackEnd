namespace FSI.PersonalFinanceApp.Application.Dtos
{
    public class IncomeCategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        public IncomeCategoryDto()
        {
                
        }

        public IncomeCategoryDto(long id)
        {
            Id = id;
        }

    }
}
