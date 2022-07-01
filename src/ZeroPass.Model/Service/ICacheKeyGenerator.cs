namespace ZeroPass.Model.Service
{
    public interface ICacheKeyGenerator
    {
        string GenerateActivationKey(string email);

        string GenerateUserKeyById(int userId);
        
        string GenerateUserKeyByEmail(string email);
        
        string GenerateExchangeKey(string keyId);
        
        string GenerateUserKeySession(int userId);
        
        string GenerateDistributionKey(int assigneeId, int assignerId);
        
        string GenerateDomainOwnerByDomainId(int domainId);
    }
}
