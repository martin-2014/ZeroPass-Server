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

        public UserKeyRepositoryFake(FakeDatabase database)
        {
            Keys = database.UserKeys;
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
    }
}
