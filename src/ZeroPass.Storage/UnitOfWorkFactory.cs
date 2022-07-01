using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace ZeroPass.Storage
{
    internal class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        readonly ConnectionOption Options;
        readonly IDomainDataState DataState;

        public UnitOfWorkFactory(ConnectionOption options, IDomainDataState dataState)
        {
            Options = options;
            DataState = dataState;
        }

        public async Task<IUnitOfWork> CreateRead(int domainId, DomainDataType types)
        {
            if (await DataState.IsDirty(domainId, types))
                return await ConnectMaster();

            return await CreateReadonly();
        }

        public Task<IUnitOfWork> CreateRead() => ConnectMaster();

        public Task<IUnitOfWork> CreateWrite() => ConnectMaster();
        
        public async Task<IUnitOfWork> CreateReadonly()
        {
            var conn = await CreateAndOpenConnection(Options.ReadonlyMysqlConnectionString);
            return new UnitOfWork(conn, DataState);
        }

        async Task<IUnitOfWork> ConnectMaster()
        {
            var conn = await CreateAndOpenConnection(Options.MasterMysqlConnectionString);
            return new UnitOfWork(conn, DataState);
        }

        async Task<MySqlConnection> CreateAndOpenConnection(string connectionString)
        {
            var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();
            return conn;
        }
    }
}
