using System.Threading.Tasks;

namespace ZeroPass.Model.Service
{
    public interface IUserService
    {
        Task<bool> ExistsUserByEmail(string email);
    }
}
