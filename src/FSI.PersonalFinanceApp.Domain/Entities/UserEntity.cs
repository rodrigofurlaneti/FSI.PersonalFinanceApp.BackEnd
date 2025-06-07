namespace FSI.PersonalFinanceApp.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
