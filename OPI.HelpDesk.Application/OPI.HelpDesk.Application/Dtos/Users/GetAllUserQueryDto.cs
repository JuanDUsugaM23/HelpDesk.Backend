namespace OPI.HelpDesk.Application.Dtos.Users
{
    public class GetAllUserQueryDto
    {
        public Guid? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Role { get; set; }
        public List<string> Specialities { get; set; } = new List<string>();
        public int? MaxTicket { get; set; }
        public bool? IsEnable { get; set; }
        public bool IsDescending { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
