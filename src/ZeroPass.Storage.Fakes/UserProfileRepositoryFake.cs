using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage.Fakes
{
    public class UserProfileRepositoryFake : IUserProfileRepository
    {
        public readonly List<UserProfileEntity> UserProfiles = new List<UserProfileEntity>();
        public readonly List<UserEntity> Users;

        public UserProfileRepositoryFake(FakeDatabase database)
        {
            UserProfiles = database.UserProfiles;
            Users = database.Users;
        }

        public Task Insert(UserProfileEntity entity)
        {
            UserProfiles.Add(entity);
            return Task.CompletedTask;
        }
        
        public Task<UserProfileView> GetProfile(int userId)
        {
            var user = Users.FirstOrDefault(u => u.Id == userId);
            var userProfile = UserProfiles.FirstOrDefault(up => up.UserId == userId);
            var view = new UserProfileView()
            {
                Id = user.Id,
                UserName = user.UserName,
                UserType = user.UserType,
                Email = user.Email,
                Timezone = userProfile.Timezone
            };
            return Task.FromResult(view);
        }
    }
}
