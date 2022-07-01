using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage.Fakes
{
    public class UserKeyDistributionRepositoryFake : IUserKeyDistributionRepository
    {
        public readonly List<UserKeyDistributionEntity> KeyDistribution;

        public UserKeyDistributionRepositoryFake(FakeDatabase database)
            => KeyDistribution = database.UserKeyDistribution;
        
        public Task<UserKeyDistributionEntity> GetByIds(int assigneeId, int assignerId)
        {
            var result = KeyDistribution.FirstOrDefault(
                k => k.AssigneeId == assigneeId && k.AssignerId == assignerId);
            return Task.FromResult(result);
        }
    }
}