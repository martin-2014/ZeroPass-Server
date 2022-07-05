using System.Threading.Tasks;
using ZeroPass.Model.Models;

namespace ZeroPass.Model.Service
{
    public partial interface IUserKeyService
    {
        Task<UserPublicKeyModel> ExchangePublicKey(UserExchangePublicKeyModel model);
        
        Task<string> Authenticate(AuthenticateModel model);
        
        Task<string> GetDataKey(IDomainActor actor, UserKeyRequestModel model);
        
        Task<bool> ActiveSession(int userId);
    }
}