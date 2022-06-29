using System;
using System.Linq;
using ZeroPass.Storage.Entities;
using ZeroPass.Storage.Fakes;

namespace ZeroPass.Api.Tests
{
    public partial class TestEnvironmentBuilder
    {
        readonly FakeDatabase Database;
        readonly TestEnvironment TestEnv;
        readonly TestSecretBuilder TestSecretBuilder = new TestSecretBuilder();

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

        TestUserSecret CreateTestUserSecret(TestUser user)
        {
            var userSecret = new TestUserSecret
            {
                UserId = user.Id,
                Email = user.Email,
                Password = Guid.NewGuid().ToString(),
                SecretKey = TestSecretBuilder.CreateSecretKey()
            };
            Database.UserSecrets.Add(userSecret);
            return userSecret;
        }

        void CreateUserKey(TestUserSecret userSecret)
            => Database.UserKeys.Add(TestSecretBuilder.CreateUserKeyEntity(
                userSecret.UserId, userSecret.Email, userSecret.Password, userSecret.SecretKey));
    }
}
