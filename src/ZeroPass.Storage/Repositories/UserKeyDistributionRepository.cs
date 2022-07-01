using System.Data;
using System.Threading.Tasks;
using Dapper;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage
{
    public class UserKeyDistributionRepository : IUserKeyDistributionRepository
    {
        readonly IDbConnection Connection;

        public UserKeyDistributionRepository(IUnitOfWork unitOfWork)
            => Connection = unitOfWork.Connection;
        
        public Task<UserKeyDistributionEntity> GetByIds(int assigneeId, int assignerId)
        {
            var sql = @"select assignee_id, assigner_id, assigner_private_data_key " +
                      "from t_user_key_distribution " +
                      "where assignee_id=@AssigneeId and assigner_id=@AssignerId";
            
            return Connection.QueryFirstOrDefaultAsync<UserKeyDistributionEntity>(
                sql,
                new { AssigneeId = assigneeId, AssignerId = assignerId });
        }
    }
}