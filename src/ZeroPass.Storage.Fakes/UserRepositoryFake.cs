using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage.Fakes
{
    public class UserRepositoryFake : IUserRepository
    {
        readonly FakeDatabase Database;

        public readonly List<UserEntity> UserEntities;

        public UserRepositoryFake(FakeDatabase database)
        {
            Database = database;
            UserEntities = database.Users;
        }

        public Task<UserEntity> GetByEmail(string email)
        {
            var user = UserEntities.FirstOrDefault(u => u.Email == email);
            return Task.FromResult(user);
        }

        public Task<int> Insert(UserEntity entity)
        {
            entity.Id = Database.AllocateDomainId();
            UserEntities.Add(entity);
            return Task.FromResult(entity.Id);
        }
        
        public Task UpdateUserName(int userId, string userName)
        {
            var user = UserEntities.FirstOrDefault(u => u.Id == userId);
            user.UserName = userName;
            return Task.CompletedTask;
        }
    }
}
