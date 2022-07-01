using System.Threading.Tasks;

namespace ZeroPass.Storage
{
    public interface IUnitOfWorkFactory
    {
        Task<IUnitOfWork> CreateRead(int domainId, DomainDataType types);
        
        Task<IUnitOfWork> CreateRead();

        Task<IUnitOfWork> CreateWrite();
        
        Task<IUnitOfWork> CreateReadonly();
    }
}
