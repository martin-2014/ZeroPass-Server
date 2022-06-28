using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage.Fakes
{
    public class UserProfileRepositoryFake : IUserProfileRepository
    {
        public readonly List<UserProfileEntity> UserProfiles = new List<UserProfileEntity>();

        public UserProfileRepositoryFake(FakeDatabase database)
        {
            UserProfiles = database.UserProfiles;
        }

        public Task Insert(UserProfileEntity entity)
        {
            UserProfiles.Add(entity);
            return Task.CompletedTask;
        }
    }
}
