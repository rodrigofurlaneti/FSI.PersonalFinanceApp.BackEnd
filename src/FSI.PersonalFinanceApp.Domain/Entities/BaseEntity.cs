namespace FSI.PersonalFinanceApp.Domain.Entities
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        protected BaseEntity()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        public void Activate() => IsActive = true;

        public void Deactivate() => IsActive = false;

        public void UpdateTimestamp() => UpdatedAt = DateTime.UtcNow;
    }
}
