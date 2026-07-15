using OPI.HelpDesk.Application.Interfaces.Repositories;
using OPI.HelpDessk.Domain.Entities;

namespace OPI.HelpDesk.Application.Interfaces.UnitOfWorks
{
    public interface IUnitOfWork :IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : AuidEntity;
        Task<int> SaveChangesAsync();
    }
}
