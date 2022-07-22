using System.Threading.Tasks;
using ZeroPass.Model.Models;

namespace ZeroPass.Model.Service
{
    public interface IClientVersionService
    {
        Task<(bool, string)> MeetMinimumVersion(ClientVersionModel version);

        Task SaveClientVersion(ClientVersionModel version);
    }
}