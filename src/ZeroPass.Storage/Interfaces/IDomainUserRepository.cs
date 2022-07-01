using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage
{
    public partial interface IDomainUserRepository
    {
        Task Upsert(DomainUserEntity entity);
        
        Task<IEnumerable<UserDomainView>> GetDomainsByUserId(int userId);
        
        Task<DomainUserEntity> GetDomainOwnerByDomainId(int domainId);
        
        Task<IEnumerable<DomainUserDetailView>> GetDomainDetailsByUserId(int userId);
    }
}
