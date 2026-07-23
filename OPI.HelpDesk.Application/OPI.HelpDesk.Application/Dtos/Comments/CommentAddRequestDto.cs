namespace OPI.HelpDesk.Application.Dtos.Comments
{
    public class CommentAddRequestDto
    {
        public Guid TicketId { get; set; }
        public string? Text { get; set; } = string.Empty;
    }
}
