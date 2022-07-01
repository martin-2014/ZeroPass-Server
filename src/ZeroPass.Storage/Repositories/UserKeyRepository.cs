using Dapper;
using System.Data;
using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage
{
    public class UserKeyRepository : IUserKeyRepository
    {
        readonly IDbConnection Connection;

        public UserKeyRepository(IUnitOfWork unitOfWork)
            => Connection = unitOfWork.Connection;

        public async Task Insert(UserKeyEntity entity)
        {
            var sql = "insert into t_user_key" +
                "(user_id, salt, verifier, public_data_key, private_data_key)" +
                "values " +
                "(@UserId, @Salt, @Verifier, @PublicDataKey, @PrivateDataKey)";

            await Connection.ExecuteAsync(sql, entity);
        }


        public Task<UserKeyEntity> GetByUserId(int userId)
        {
            var sql = "select user_id, salt, verifier, public_data_key, private_data_key " +
                "from t_user_key " +
                "where user_id = @UserId";

            return Connection.QueryFirstOrDefaultAsync<UserKeyEntity>(sql, new { UserId = userId });
        }

        public async Task<UserKeyEntity> GetByEmail(string email)
        {
            var sql = "select user_id, salt, verifier, public_data_key, private_data_key " +
                      "from t_user_key k inner join " +
                      "t_user u on k.user_id = u.id " +
                      "where u.email = @Email";
            
            return await Connection.QueryFirstOrDefaultAsync<UserKeyEntity>(sql, new { Email = email });
        }

    }
}
