using OPI.HelpDessk.Domain.Entities.Enums;

namespace OPI.HelpDessk.Domain.Entities
{
    public class SettingSLA : AuidEntity
    {
        public TicketPriorityEnum Priority { get; set; }
        public TicketCategoriesEnum Categority { get; set; }
        public double LimitTime { get; set; }

    }
}
