using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace ZeroPass.Storage
{
    internal class UnitOfWork : IUnitOfWork
    {
        UserRepository users;
        DomainRepository domains;
        DomainUserRepository domainUsers;
        UserProfileRepository userProfiles;
        UserKeyRepository keys;
        UserKeyDistributionRepository keyDistributions;
        NotificationRepository notifications;

        public MySqlConnection MySqlConnection { get; private set; }
        MySqlTransaction MySqlTransaction;
        readonly IDomainDataState DataState;

        public UnitOfWork(MySqlConnection connection, IDomainDataState dataState)
        {
            MySqlConnection = connection;
            DataState = dataState;
        }

        public IDbConnection Connection => MySqlConnection;
        public IUserRepository Users => users ??= new UserRepository(this);
        public IDomainRepository Domains => domains ??= new DomainRepository(this);
        public IDomainUserRepository DomainUsers => domainUsers ??= new DomainUserRepository(this);
        public IUserProfileRepository UserProfiles => userProfiles ??= new UserProfileRepository(this);
        public IUserKeyRepository UserKeys => keys ??= new UserKeyRepository(this);
        public IUserKeyDistributionRepository UserKeyDistributions => keyDistributions ??= new UserKeyDistributionRepository(this);
        public INotificationRepository Notifications => notifications ??= new NotificationRepository(this);

        public async Task BeginTrans()
        {
            if (MySqlTransaction != null)
                throw new Exception("A transaction has been opened.");

            MySqlTransaction = await MySqlConnection.BeginTransactionAsync();
        }

        public async Task CommitTrans()
        {
            if (MySqlTransaction == null)
                throw new Exception("No transaction opened");

            await MySqlTransaction.CommitAsync();
            await DisposeTrans();
        }

        public async Task RollbackTrans()
        {
            if (MySqlTransaction == null)
                throw new Exception("No transaction opened");

            await MySqlTransaction.RollbackAsync();
            await DisposeTrans();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                MySqlConnection?.Dispose();
                MySqlConnection = null;
            }
        }

        async ValueTask DisposeAsyncCore()
        {
            if (MySqlConnection != null)
            {
                await MySqlConnection.DisposeAsync().ConfigureAwait(false);
            }

            MySqlConnection = null;
        }

        async Task DisposeTrans()
        {
            if (MySqlTransaction == null) return;

            var tran = MySqlTransaction;
            MySqlTransaction = null;
            await tran.DisposeAsync();
        }
    }
}
