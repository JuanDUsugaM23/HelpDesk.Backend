using OPI.HelpDessk.Domain.Entities.Enums;

namespace OPI.HelpDesk.Application.Dtos.Tickets
{
    public class GetAllTicketsQueryDto
    {
        public string? Search { get; set; }
        public string? Category { get; set; }
        public string? Priority { get; set; }
        public string? Status { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? AssignedTechnicianId { get; set; }
        public bool? IsOverdue { get; set; }
        public bool IsDescending { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}