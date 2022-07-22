using System.Data;
using System.Threading.Tasks;
using Dapper;
using ZeroPass.Storage.Entities.ClientVersions;

namespace ZeroPass.Storage
{
    internal class ClientVersionRepository : IClientVersionRepository
    {
        readonly IDbConnection Connection;

        public ClientVersionRepository(IUnitOfWork unitOfWork) => Connection = unitOfWork.Connection;

        public Task<ClientMinVersionEntity> GetMinRequiredVersionByEdition(string edition)
        {
            var query = @"SELECT 
	                    v.edition, 
                        v.min_version
                    FROM t_min_client_version v
                    WHERE v.edition = @Edition";
            
            return Connection.QueryFirstOrDefaultAsync<ClientMinVersionEntity>(query, new { Edition = edition });
        }

        public async Task SaveClientVersion(ClientVersionView version)
        {
            var sql = @"INSERT INTO t_client_version
                      (user_id, edition, version, device_id)
                      VALUES
                      ((SELECT id  FROM t_user WHERE email=@Email), @Edition, @Version, @DeviceId)
                      ON DUPLICATE KEY UPDATE
                      edition=@Edition, version=@Version;";
            
            await Connection.ExecuteAsync(sql, version);
        }
    }
}