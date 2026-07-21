using System.Security.Principal;

namespace OPI.HelpDesk.Application.Dtos.SettingSLAs
{
    public class SettingSLAResponseDto
    {
        public Guid Id { get; set; }
        public string Priority { get; set; }
        public string Category { get; set; }
        public double LimitTime { get; set; }
        public bool Enable { get; set; }
    }
}
