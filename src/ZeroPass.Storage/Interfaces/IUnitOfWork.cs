using System;
using System.Data;

namespace ZeroPass.Storage
{
    public interface IUnitOfWork : IRepositoryFactory, IDisposable, IAsyncDisposable
    {
        IDbConnection Connection { get; }
    }
}
