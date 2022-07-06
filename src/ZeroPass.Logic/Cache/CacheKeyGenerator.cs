using ZeroPass.Model.Service;

namespace ZeroPass.Service
{
    public partial class CacheKeyGenerator : ICacheKeyGenerator
    {
        const string ActivationPrefix = "Activation";
        const string UserKeyPrefix = "UserKey";
        const string UserExchangeKeyPrefix = "UserExchangeKey";
        const string UserSessionPrefix = "UserSession";
        const string UserDomainOwnerPrefix = "DomainOwner";


        public string GenerateActivationKey(string email)
            => $"{ActivationPrefix}@{email.ToLower()}";

        public string GenerateUserKeyById(int userId)
            => $"{UserKeyPrefix}@{userId}";

        public string GenerateUserKeyByEmail(string email)
            => $"{UserKeyPrefix}@{email.ToLower()}";
        
        public string GenerateExchangeKey(string keyId)
            => $"{UserExchangeKeyPrefix}@{keyId}";

        public string GenerateUserKeySession(int userId, string deviceId)
            => $"{UserSessionPrefix}@{userId}@{deviceId}";

        public string GenerateDomainOwnerByDomainId(int domainId)
            => $"{UserDomainOwnerPrefix}@{domainId}";
        
    }
}
