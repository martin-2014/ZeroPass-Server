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

        public Task<UserProfileView> GetProfile(int userId)
        {
            var query = @"SELECT 
	                    u.id, 
                        u.user_type,
                        u.user_name, 
                        u.email, 
                        up.timezone
                    FROM t_user u INNER JOIN 
	                    t_user_profile up ON u.id = up.user_id
                    WHERE u.id = @UserId";
            
            return Connection.QueryFirstOrDefaultAsync<UserProfileView>(query, new { UserId = userId });
        }
    }
}
