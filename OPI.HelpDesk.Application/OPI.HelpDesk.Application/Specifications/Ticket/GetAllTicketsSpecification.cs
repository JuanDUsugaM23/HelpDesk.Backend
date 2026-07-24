using OPI.HelpDesk.Application.Dtos.Tickets;
using OPI.HelpDesk.Application.Specifications;
using OPI.HelpDessk.Domain.Entities;
using OPI.HelpDessk.Domain.Entities.Enums;

namespace OPI.HelpDesk.Application.Specifications.Tickets
{
    public class GetAllTicketsSpecification : BaseSpecification<Ticket>
    {
        public GetAllTicketsSpecification(GetAllTicketsQueryDto query)
        {
            AddInclude(t => t.Client);
            AddInclude(t => t.AssignedTechnical);

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                AddCriteria(t =>
                    t.Title!.ToLower().Contains(query.Search.ToLower()) ||
                    t.Description!.ToLower().Contains(query.Search.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                TicketCategoriesEnum category =
                    (TicketCategoriesEnum)Enum.Parse(
                        typeof(TicketCategoriesEnum),
                        query.Category,
                        true);

                AddCriteria(t => t.Category == category);
            }

            if (!string.IsNullOrWhiteSpace(query.Priority))
            {
                TicketPriorityEnum priority =
                    (TicketPriorityEnum)Enum.Parse(
                        typeof(TicketPriorityEnum),
                        query.Priority,
                        true);

                AddCriteria(t => t.Priority == priority);
            }

            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                TicketStatusEnum status =
                    (TicketStatusEnum)Enum.Parse(
                        typeof(TicketStatusEnum),
                        query.Status,
                        true);

                AddCriteria(t => t.Status == status);
            }

            if (query.ClientId.HasValue)
            {
                AddCriteria(t => t.Client.Id == query.ClientId.Value);
            }

            if (query.AssignedTechnicianId.HasValue)
            {
                AddCriteria(t => t.AssignedTechnicalId == query.AssignedTechnicianId.Value);
            }

            if (query.IsOverdue.HasValue)
            {
                AddCriteria(t => t.IsOverdue == query.IsOverdue.Value);
            }

            if (query.IsDescending)
            {
                ApplyOrderByDescending(t => t.CreatedAt);
            }

            else
            {
                ApplyOrderBy(t => t.CreatedAt);
            }

            ApplyPaging(
            (query.Page - 1) * query.PageSize,
            query.PageSize);
        }
    }
}