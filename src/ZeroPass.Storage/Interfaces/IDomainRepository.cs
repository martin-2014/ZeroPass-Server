using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage
{
    public interface IDomainRepository
    {
        Task<int> Insert(DomainEntity entity);

        Task InsertDomainInfo(DomainInfoEntity entity);
    }
}
