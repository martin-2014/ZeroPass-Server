using System.Linq;
using ZeroPass.Storage.Entities;
using ZeroPass.Storage.Fakes;

namespace ZeroPass.Api.Tests
{
    public class TestEnvironmentBuilder
    {
        readonly FakeDatabase Database;
        readonly TestEnvironment TestEnv;

        public TestEnvironmentBuilder(TestEnvironment testEnv)
        {
            TestEnv = testEnv;
            Database = testEnv.Database;
        }

        public void CreatePersonalUsers(int count) 
            => Enumerable
                .Range(0, count)
                .Select(_ => CreatePersonalUser())
                .ToList();

        public UserEntity CreatePersonalUser()
        {
            var userId = Database.AllocateUserId();
            var user = new UserEntity
            {
                Id = userId,
                Email = $"User{userId}@personal.com",
                UserName = $"User{userId}",
                UserType = 2,
            };
            Database.Users.Add(user);
            return user;
        }
    }
}
