namespace OPI.HelpDesk.Application.Dtos.Comments
{
    public class CommentEditRequestDto
    {
        public Guid Id { get; set; }
        public Guid? TicketId { get; set; }
        public string? Text { get; set; } = string.Empty;
        public bool? Enable { get; set; }
    }
}
