using System;
using System.Data;
using System.Threading.Tasks;

namespace ZeroPass.Storage
{
    public interface IUnitOfWork : IRepositoryFactory, IDisposable, IAsyncDisposable
    {
        IDbConnection Connection { get; }

        Task BeginTrans();

        Task CommitTrans();

        Task RollbackTrans();
    }
}
