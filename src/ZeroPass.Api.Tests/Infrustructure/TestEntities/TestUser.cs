using System.Collections.Generic;
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
        
        public TestUserSecret UserSecret => Database.UserSecrets.First(s => s.UserId == Id);
        
        public IEnumerable<TestNotification> Notifications => 
            Database.Notifications
                .Where(n => n.UserId == Id)
                .Select(e => new TestNotification(e));

        public TestDomain PersonalDomain
        {
            get
            {
                var domainIds = Database.DomainUsers.Where(u => u.UserId == Id && u.IsActive).Select(u => u.DomainId);
                var domains = Database.Domains.Where(d => domainIds.Contains(d.Id) && d.DomainType == DomainType.Personal);
                var domainId = domains.Single().Id;
                return new TestDomain(domainId, TestEnv);
            }
        }
    }
}
