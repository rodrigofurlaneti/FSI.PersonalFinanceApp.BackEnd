namespace FSI.PersonalFinanceApp.Application.Dtos
{
    public class TrafficDto
    {
        public long Id { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public DateTime? BackEndCreatedAt { get; set; } // para manter rastreamento, data e hora de criação no back-end
        public DateTime? CreatedAt { get; set; } // para manter rastreamento, opcional
        public DateTime? UpdatedAt { get; set; } // para rastreamento
        public bool IsActive { get; set; } = true;

        public TrafficDto()
        {
        }

        public TrafficDto(string method, string action, DateTime backEndCreatedAt)
        {
            Method = method;
            Action = action;
            BackEndCreatedAt = backEndCreatedAt;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            IsActive = true;
        }
    }
}
