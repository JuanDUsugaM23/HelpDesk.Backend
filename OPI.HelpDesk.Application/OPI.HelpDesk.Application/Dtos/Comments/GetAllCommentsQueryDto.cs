namespace OPI.HelpDesk.Application.Dtos.Comments
{
    public class GetAllCommentsQueryDto
    {
        public Guid? Id { get; set; }
        public Guid? TicketId { get; set; }
        public bool? IsEnable { get; set; }
        public bool IsDescending { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

    }
}
