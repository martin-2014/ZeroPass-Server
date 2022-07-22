using System.Threading.Tasks;
using ZeroPass.Model.Models;
using ZeroPass.Storage.Entities.ClientVersions;

namespace ZeroPass.Storage
{
    public interface IClientVersionRepository
    {
        Task<ClientMinVersionEntity> GetMinRequiredVersionByEdition(string edition);
        Task SaveClientVersion(ClientVersionView version);
    }
}