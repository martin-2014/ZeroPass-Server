using System.Threading.Tasks;

namespace ZeroPass.Storage
{
    public interface IDomainDataState
    {
        Task SetDirty(int domainId, DomainDataType types);

        Task<bool> IsDirty(int domainId, DomainDataType types);
    }
}