using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage.Fakes
{
    public class UserRepositoryFake : IUserRepository
    {

        public readonly List<UserEntity> UserEntities;

        public UserRepositoryFake(FakeDatabase database) => UserEntities = database.Users;

        public Task<UserEntity> GetByEmail(string email)
        {
            var user = UserEntities.FirstOrDefault(u => u.Email == email);
            return Task.FromResult(user);
        }
    }
}
