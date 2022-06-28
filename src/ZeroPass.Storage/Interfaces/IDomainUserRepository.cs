using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage
{
    public interface IDomainUserRepository
    {
        Task Upsert(DomainUserEntity entity);
    }
}
