namespace OPI.HelpDesk.Application.Dtos.SettingSLAs
{
    public class GetSettingSLAsQueryDto
    {
        public Guid? Id { get; set; }
        public string? Priority { get; set; }
        public string? Category { get; set; }
        public double? LimitTime { get; set; }
        public bool? IsEnble { get; set; }
        public bool IsDescending { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
