using System;
using System.Data;
using System.Threading.Tasks;

namespace ZeroPass.Storage.Fakes
{
    public class UnitOfWorkFake : IUnitOfWork
    {
        readonly FakeDatabase Database;
        Action ReleaseConnection;
        public UnitOfWorkFake(FakeDatabase database, Action releaseConnection)
            => (Database, ReleaseConnection) = (database, releaseConnection);

        public IDbConnection Connection => null;

        public IUserRepository Users => new UserRepositoryFake(Database);

        public void Dispose() => ReleaseConnection();

        public ValueTask DisposeAsync() => new ValueTask();
    }
}
