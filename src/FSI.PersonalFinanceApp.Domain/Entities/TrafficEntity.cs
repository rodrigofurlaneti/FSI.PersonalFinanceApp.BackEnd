namespace FSI.PersonalFinanceApp.Domain.Entities
{
    public class TrafficEntity : BaseEntity
    {
        public string Method { get; set; }
        public string Action { get; set; }
        public DateTime? BackEndCreatedAt { get; set; } // para manter rastreamento, data e hora de criação no back-end
    }
}
