using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage
{
    public interface IUserKeyRepository
    {
        Task Insert(UserKeyEntity entity);

        Task<UserKeyEntity> GetByUserId(int userId);
        
        Task<UserKeyEntity> GetByEmail(string email);
    }
}