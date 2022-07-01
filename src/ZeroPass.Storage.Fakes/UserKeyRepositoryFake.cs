using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage.Fakes
{
    public class UserKeyRepositoryFake : IUserKeyRepository
    {
        public readonly List<UserKeyEntity> Keys;
        public readonly List<UserEntity> Users;

        public UserKeyRepositoryFake(FakeDatabase database)
        {
            Keys = database.UserKeys;
            Users = database.Users;
        }

        public Task<UserKeyEntity> GetByUserId(int userId)
        {
            var result = Keys.FirstOrDefault(k => k.UserId == userId);
            return Task.FromResult(result);
        }

        public Task Insert(UserKeyEntity entity)
        {
            Keys.Add(entity);
            return Task.CompletedTask;
        }
        
        public Task<UserKeyEntity> GetByEmail(string email)
        {
            var user = Users.FirstOrDefault(u => u.Email == email);
            
            if (user == null) 
                return Task.FromResult<UserKeyEntity>(null);
            
            var userKey = Keys.FirstOrDefault(up => up.UserId == user.Id);
            
            return Task.FromResult(userKey);
        }
    }
}
