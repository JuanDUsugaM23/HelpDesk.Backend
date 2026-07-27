using OPI.HelpDessk.Domain.Entities;

namespace OPI.HelpDesk.Application.Specifications.Users;

public class UserByIdSpecification : BaseSpecification<User>
{
    public UserByIdSpecification(Guid id)
    {
        AddCriteria(u => u.Id == id);
    }
}