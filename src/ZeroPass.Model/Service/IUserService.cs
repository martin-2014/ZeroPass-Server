using System.Threading.Tasks;
using ZeroPass.Model.Models;

namespace ZeroPass.Model.Service
{
    public partial interface IUserService
    {
        Task<bool> ExistsUserByEmail(string email);

        Task RegisterUser(UserRegisterModel user);
    }
}
