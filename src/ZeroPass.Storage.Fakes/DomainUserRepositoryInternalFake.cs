namespace ZeroPass.Storage.Fakes
{
    public partial class DomainUserRepositoryFake
    {
        private void Initialize(FakeDatabase database)
        {
            DomainUserEntities = database.DomainUsers;
            DomainEntities = database.Domains;
            DomainInfoEntities = database.DomainInfos;
            UserEntities = database.Users;
        }
    }
}