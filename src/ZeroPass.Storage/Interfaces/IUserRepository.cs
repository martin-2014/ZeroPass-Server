using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage
{
    public interface IUserRepository
    {
        Task<UserEntity> GetByEmail(string email);

        Task<int> Insert(UserEntity entity);
    }
}
