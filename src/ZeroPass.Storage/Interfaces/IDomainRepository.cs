using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage
{
    public partial interface IDomainRepository
    {
        Task<int> Insert(DomainEntity entity);

        Task InsertDomainInfo(DomainInfoEntity entity);

        Task<DomainEntity> GetDomainByName(string domainName);
        
        Task UpdateDomainLogo(DomainInfoEntity entity);
        
        Task<IEnumerable<DomainEntity>> GetDomainByIds(IEnumerable<int> ids);
        
        Task<DomainEntity> GetDomainById(int id);
    }
}
