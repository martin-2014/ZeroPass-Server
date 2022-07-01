using System.Threading.Tasks;
using ZeroPass.Model.Models;

namespace ZeroPass.Model.Service
{
    public partial interface ITokenService
    {
        Task<string> Authenticate(string email);
        
        Task<string> RefreshToken(TokenModel token);
    }
}