using System;
using System.Threading.Tasks;

namespace ZeroPass.Storage.Fakes
{
    public class UnitOfWorkFactoryFake : IUnitOfWorkFactory
    {
        readonly FakeDatabase Database;
        const int MaxPoolSise = 1;
        int CurrentPoolSize = 0;

        public UnitOfWorkFactoryFake(FakeDatabase database) => Database = database;

        public async Task<IUnitOfWork> CreateRead()
        {
            CheckPoolAvailable();
            CurrentPoolSize++;
            return await Task.FromResult(new UnitOfWorkFake(Database, ReleaseConnection));
        }

        public async Task<IUnitOfWork> CreateWrite()
        {
            CheckPoolAvailable();
            CurrentPoolSize++;
            return await Task.FromResult(new UnitOfWorkFake(Database, ReleaseConnection));
        }

        void CheckPoolAvailable()
        {
            if (CurrentPoolSize >= MaxPoolSise)
            {
                throw new TimeoutException("Timeout expired.  The timeout period elapsed prior to obtaining a connection from the pool.");
            }
        }

        void ReleaseConnection()
            => CurrentPoolSize--;
    }
}
