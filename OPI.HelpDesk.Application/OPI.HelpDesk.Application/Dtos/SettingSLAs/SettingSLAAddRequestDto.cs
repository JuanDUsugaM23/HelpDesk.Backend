namespace OPI.HelpDesk.Application.Dtos.SettingSLAs
{
    public class SettingSLAAddRequestDto
    {
        public string Priority { get; set; }
        public string Category { get; set; }
        public double LimitTime { get; set; }
    }
}
