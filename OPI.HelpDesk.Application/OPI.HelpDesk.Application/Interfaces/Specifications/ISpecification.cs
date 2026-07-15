using OPI.HelpDessk.Domain.Entities;
using System.Linq.Expressions;

namespace OPI.HelpDesk.Application.Interfaces.Specifications
{
    public interface ISpecification<T> where T : AuidEntity
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        int? Skip { get; }
        int? Take { get; }
    }
}
