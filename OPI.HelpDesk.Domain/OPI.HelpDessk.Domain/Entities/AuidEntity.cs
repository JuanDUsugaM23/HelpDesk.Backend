namespace OPI.HelpDessk.Domain.Entities
{
    public class AuidEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid UpdatedBy { get; set; }
        public bool Enable { get; set; }
    }
}
