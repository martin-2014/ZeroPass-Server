namespace ZeroPass.Model.Service
{
    public partial interface ICacheKeyGenerator
    {
        string GenerateActivationKey(string email);

        string GenerateUserKeyById(int userId);
        
        string GenerateUserKeyByEmail(string email);
        
        string GenerateExchangeKey(string keyId);
        
        string GenerateUserKeySession(int userId, string deviceId);

        string GenerateDomainOwnerByDomainId(int domainId);
    }
}
