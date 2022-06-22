using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace ZeroPass.Storage
{
    internal class UnitOfWork : IUnitOfWork
    {
        UserRepository users;

        public MySqlConnection MySqlConnection { get; private set; }

        public UnitOfWork(MySqlConnection connection) => MySqlConnection = connection;

        public IDbConnection Connection => MySqlConnection;
        public IUserRepository Users => users ??= new UserRepository(this);

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
    }
}
