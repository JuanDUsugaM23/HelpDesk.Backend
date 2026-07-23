namespace OPI.HelpDesk.Application.Dtos.Comments
{
    public class CommentResponseDto
    {
        public Guid Id { get; set; }
        public Guid TicketId { get; set; }
        public string TickedTitle { get; set; }
        public string Text { get; set; }
        public bool Enable { get; set; }
    }
}
