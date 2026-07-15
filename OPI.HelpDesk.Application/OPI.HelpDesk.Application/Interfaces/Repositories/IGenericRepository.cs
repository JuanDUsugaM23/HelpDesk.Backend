using OPI.HelpDesk.Application.Interfaces.Specifications;
using OPI.HelpDessk.Domain.Entities;

namespace OPI.HelpDesk.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : AuidEntity
    {
        Task<T?> GetAsync(ISpecification<T> spec, CancellationToken ct = default);
        Task<IEnumerable<T>> GetAllAsync(ISpecification<T> spec, CancellationToken ct = default);
        Task AddAsync(T entity, CancellationToken ct = default);
        void Update(T entity);
        void Delete(T entity);
        Task<int> CountAsync(CancellationToken ct = default);
    }
}
