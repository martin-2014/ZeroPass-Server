using System.Threading.Tasks;
using ZeroPass.Model.Models;

namespace ZeroPass.Model.Service
{
    public partial interface IUserProfileService
    {
        Task<UserProfileModel> GetUserProfile(IActor actor);
        
        Task UpdateUserProfile(IDomainActor actor, UserProfileUpdateModel request);
    }
}