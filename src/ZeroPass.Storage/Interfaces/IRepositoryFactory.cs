namespace ZeroPass.Storage
{
    public interface IRepositoryFactory
    {
        IUserRepository Users { get; }
    }
}
