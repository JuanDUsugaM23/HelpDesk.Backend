namespace OPI.HelpDessk.Domain.Entities
{
    public class RefreshToken : AuidEntity
    {
        public string? Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
