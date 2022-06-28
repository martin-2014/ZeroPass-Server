using ZeroPass.Model.Service;

namespace ZeroPass.Service
{
    public class CacheKeyGenerator : ICacheKeyGenerator
    {
        const string ActivationPrefix = "Activation";
        const string UserKeyPrefix = "UserKey";

        public string GenerateActivationKey(string email)
            => $"{ActivationPrefix}@{email.ToLower()}";

        public string GenerateUserKeyById(int userId)
            => $"{UserKeyPrefix}@{userId}";
    }
}
