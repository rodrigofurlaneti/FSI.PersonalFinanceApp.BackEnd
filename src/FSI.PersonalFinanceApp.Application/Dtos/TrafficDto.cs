namespace FSI.PersonalFinanceApp.Application.Dtos
{
    public class TrafficDto
    {
        public long Id { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; } // para manter rastreamento, opcional
        public DateTime? UpdatedAt { get; set; } // para rastreamento
        public bool IsActive { get; set; } = true;

        public TrafficDto()
        {
        }

        public TrafficDto(string method, string action)
        {
            Method = method;
            Action = action;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            IsActive = true;
        }
    }
}
