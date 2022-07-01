using System.Linq;
using ZeroPass.Storage.Entities;
using ZeroPass.Storage.Fakes;

namespace ZeroPass.Api.Tests
{
    public partial class TestDomain
    {
        readonly TestEnvironment TestEnv;
        readonly FakeDatabase Database;
        readonly DomainEntity domainInDb;
        
        public TestDomain(
            int domainId,
            TestEnvironment testEnv)
        {
            TestEnv = testEnv;
            Database = testEnv.Database;
            domainInDb = Database.Domains.FirstOrDefault(d => d.Id == domainId);
        }
        
        public DomainEntity Domain => domainInDb;
        
        public int Id { get => Domain.Id; }
    }
}