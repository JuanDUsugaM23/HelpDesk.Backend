using OPI.HelpDesk.Application.Dtos.Users;
using OPI.HelpDessk.Domain.Entities;
using OPI.HelpDessk.Domain.Entities.Enums;

namespace OPI.HelpDesk.Application.Specifications.Users
{
    public class UserSpecificationFilter : BaseSpecification<User>
    {
        public UserSpecificationFilter(GetAllUserQueryDto query)
        {
            if (query.IsEnable.HasValue)
                AddCriteria(u => u.Enable == query.IsEnable);

            if (!string.IsNullOrWhiteSpace(query.FirstName))
                AddCriteria(u => u.FirstName.ToLower().Contains(query.FirstName.ToLower()));

            if (!string.IsNullOrWhiteSpace(query.LastName))
                AddCriteria(u => u.LastName.ToLower().Contains(query.LastName.ToLower()));
            if (!string.IsNullOrWhiteSpace(query.Role))
            {
                RolesEnum roleEnum = (RolesEnum)Enum.Parse(typeof(RolesEnum), query.Role, true);
                AddCriteria(u => u.Role == roleEnum);
            }

            if(query.Specialities.Count != 0)
            {
                List<TicketCategoriesEnum> specialities = query.Specialities
                    .Select(s => (TicketCategoriesEnum)Enum.Parse(typeof(TicketCategoriesEnum), s, true))
                    .ToList();
                AddCriteria(u => u.Specialities.Any(s => specialities.Contains(s)));
            }

            if (query.IsDescending)
                ApplyOrderByDescending(b => b.Email);
            else
                ApplyOrderBy(b => b.Email);
            ApplyPaging((query.Page - 1) * query.PageSize, query.PageSize);
        }
    }
}
