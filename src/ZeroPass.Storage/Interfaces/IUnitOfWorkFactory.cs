using System.Threading.Tasks;

namespace ZeroPass.Storage
{
    public interface IUnitOfWorkFactory
    {
        Task<IUnitOfWork> CreateRead();

        Task<IUnitOfWork> CreateWrite();
    }
}
