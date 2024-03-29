﻿using System;
using System.Data;
using System.Threading.Tasks;

namespace ZeroPass.Storage.Fakes
{
    public partial class UnitOfWorkFake : IUnitOfWork
    {
        readonly FakeDatabase Database;
        Action ReleaseConnection;
        public UnitOfWorkFake(FakeDatabase database, Action releaseConnection)
            => (Database, ReleaseConnection) = (database, releaseConnection);

        public IDbConnection Connection => null;

        public IUserRepository Users => new UserRepositoryFake(Database);
        public IDomainRepository Domains => new DomainRepositoryFake(Database);
        public IDomainUserRepository DomainUsers => new DomainUserRepositoryFake(Database);
        public IUserProfileRepository UserProfiles => new UserProfileRepositoryFake(Database);
        public IUserKeyRepository UserKeys => new UserKeyRepositoryFake(Database);
        public INotificationRepository Notifications => new NotificationRepositoryFake(Database);
        public IClientVersionRepository ClientVersions => new ClientVersionRepositoryFake(Database);

        public Task BeginTrans() => Task.CompletedTask;

        public Task CommitTrans() => Task.CompletedTask;

        public Task RollbackTrans() => Task.CompletedTask;

        public void Dispose() => ReleaseConnection();

        public ValueTask DisposeAsync() => new ValueTask();
        
        public Task SetDirty(int domainId, DomainDataType types)
        {
            return Task.CompletedTask;
        }
    }
}
