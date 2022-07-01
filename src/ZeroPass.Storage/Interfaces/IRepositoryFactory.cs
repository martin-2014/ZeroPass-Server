namespace ZeroPass.Storage
{
    public partial interface IRepositoryFactory
    {
        IUserRepository Users { get; }

        IDomainRepository Domains { get; }

        IDomainUserRepository DomainUsers { get; }

        IUserProfileRepository UserProfiles { get; }

        IUserKeyRepository UserKeys { get; }
        
        INotificationRepository Notifications { get; }
    }
}
