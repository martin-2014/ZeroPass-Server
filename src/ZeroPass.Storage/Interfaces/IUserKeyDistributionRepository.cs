using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage
{
    public interface IUserKeyDistributionRepository
    {
        public Task<UserKeyDistributionEntity> GetByIds(int assigneeId, int assignerId);
    }
}