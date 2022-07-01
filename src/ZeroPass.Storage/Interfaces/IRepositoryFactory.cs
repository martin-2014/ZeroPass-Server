namespace ZeroPass.Storage
{
    public interface IRepositoryFactory
    {
        IUserRepository Users { get; }

        IDomainRepository Domains { get; }

        IDomainUserRepository DomainUsers { get; }

        IUserProfileRepository UserProfiles { get; }

        IUserKeyRepository UserKeys { get; }
        
        IUserKeyDistributionRepository UserKeyDistributions { get; }
    }
}
