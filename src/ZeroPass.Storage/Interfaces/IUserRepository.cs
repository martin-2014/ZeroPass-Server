using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage
{
    public partial interface IUserRepository
    {
        Task<UserEntity> GetByEmail(string email);

        Task<int> Insert(UserEntity entity);
        
        Task UpdateUserName(int userId, string userName);
        
        Task<UserEntity> GetUserById(int id);

    }
}
