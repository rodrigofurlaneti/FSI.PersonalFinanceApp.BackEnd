namespace FSI.PersonalFinanceApp.Application.Dtos
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        public UserDto()
        {
                
        }

        public UserDto(long id)
        {
            Id = id;
        }
    }
}
