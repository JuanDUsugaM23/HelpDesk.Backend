using OPI.HelpDesk.Application.Dtos.SettingSLAs;
using OPI.HelpDessk.Domain.Entities;
using OPI.HelpDessk.Domain.Entities.Enums;

namespace OPI.HelpDesk.Application.Specifications.SettingSLAs
{
    public class SettingSLASpecificatoFilter : BaseSpecification<SettingSLA>
    {
        public SettingSLASpecificatoFilter(GetSettingSLAsQueryDto query)
        {
            if (query.IsEnble.HasValue)
                AddCriteria(ss => ss.Enable == query.IsEnble);
            if (!string.IsNullOrWhiteSpace(query.Priority))
            {
                var priority = (TicketPriorityEnum)Enum.Parse(typeof(TicketPriorityEnum), query.Priority, true);
                AddCriteria(ss => ss.Priority == priority);
            }

            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                var category = (TicketCategoriesEnum)Enum.Parse(typeof(TicketCategoriesEnum), query.Category, true);
                AddCriteria(ss => ss.Categority == category);
            }

            if (query.LimitTime.HasValue)
                AddCriteria(ss => ss.LimitTime == query.LimitTime.Value);

            if (query.IsDescending)
                ApplyOrderByDescending(ss => ss.LimitTime);
            else
                ApplyOrderBy(ss => ss.LimitTime);

            ApplyPaging((query.Page - 1) * query.PageSize, query.PageSize);

        }
    }
}
