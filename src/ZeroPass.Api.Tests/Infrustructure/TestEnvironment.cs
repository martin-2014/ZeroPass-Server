using System.Collections.Generic;
using System.Linq;
using ZeroPass.Storage.Entities;
using ZeroPass.Storage.Fakes;

namespace ZeroPass.Api.Tests
{
    public partial class TestEnvironment
    {
        public readonly FakeDatabase Database;

        public TestEnvironment(FakeDatabase database) => Database = database;

        public IEnumerable<TestUser> Users
            => Database.Users.Select(u => new TestUser(u.Id, this));

        public IEnumerable<DomainUserEntity> DomainUsers => Database.DomainUsers;
    }
}
