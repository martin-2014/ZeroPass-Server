using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage
{
    public interface IUserProfileRepository
    {
        Task Insert(UserProfileEntity entity);
    }
}
