using System.Linq;
using ZeroPass.Storage.Entities;
using ZeroPass.Storage.Fakes;

namespace ZeroPass.Api.Tests
{
    public partial class TestUser
    {
        public readonly TestEnvironment TestEnv;
        public readonly int Id;

        public TestUser(int id, TestEnvironment env)
        {
            Id = id;
            TestEnv = env;
        }

        public string Email => Entity.Email;
        public UserEntity Entity => Database.Users.First(u => u.Id == Id);
        FakeDatabase Database => TestEnv.Database;
    }
}
