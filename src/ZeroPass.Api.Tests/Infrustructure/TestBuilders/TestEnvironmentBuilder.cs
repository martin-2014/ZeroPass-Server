using System;
using System.Linq;
using ZeroPass.Model.Models;
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
                .Select(_ =>
                {
                    var user = CreatePersonalUser();
                    var domain = CreatePersonalDomain();
                    CreatePersonalDomainUser(user.Id, domain.Id);
                    CreateProfile(user.Id);

                    var testUser = new TestUser(user.Id, TestEnv);
                    var userSecret = CreateTestUserSecret(testUser);
                    CreateUserKey(userSecret);

                    return testUser;
                })
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
        
        TestDomain CreatePersonalDomain()
        {
            var domainId = Database.AllocateDomainId();
            Database.Domains.Add(new DomainEntity
            {
                Id = domainId,
                DomainName = $"Personal{domainId}",
                Company = $"Personal{domainId}",
                DomainType = DomainType.Personal,
            });
            Database.DomainInfos.Add(new DomainInfoEntity
            {
                DomainId = domainId,
                Timezone = "+8:00"
            });
            return new TestDomain(domainId, TestEnv);
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
        
        DomainUserEntity CreatePersonalDomainUser(int userId, int domainId)
        {
            var domainUser = new DomainUserEntity
            {
                DomainId = domainId,
                UserId = userId,
                IsOwner = false,
                IsAdmin = false,
                Status = UserStatus.Active,
                CreatedBy = -1,
                CreateTime = System.DateTime.MinValue
            };
            Database.DomainUsers.Add(domainUser);
            return domainUser;
        }
        
        void CreateProfile(int userId)
            => Database.UserProfiles.Add(new UserProfileEntity
            {
                UserId = userId,
                Timezone = "+8:00"
            });
    }
}
