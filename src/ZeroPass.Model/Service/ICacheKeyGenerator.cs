namespace ZeroPass.Model.Service
{
    public interface ICacheKeyGenerator
    {
        string GenerateActivationKey(string email);

        string GenerateUserKeyById(int userId);
    }
}
