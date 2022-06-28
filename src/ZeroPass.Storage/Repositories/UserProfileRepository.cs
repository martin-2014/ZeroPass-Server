using Dapper;
using System.Data;
using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage
{
    internal class UserProfileRepository : IUserProfileRepository
    {
        readonly IDbConnection Connection;

        public UserProfileRepository(IUnitOfWork unitOfWork) => Connection = unitOfWork.Connection;

        public async Task Insert(UserProfileEntity entity)
        {
            var sql = "INSERT INTO t_user_profile" +
                "(user_id, timezone)" +
                "VALUES " +
                "(@UserId, @Timezone);";
            await Connection.ExecuteAsync(sql, entity);
        }

    }
}
