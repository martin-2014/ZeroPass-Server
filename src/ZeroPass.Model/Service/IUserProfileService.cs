using System.Threading.Tasks;
using ZeroPass.Model.Models;
using ZeroPass.Model.Models.UserProfiles;

namespace ZeroPass.Model.Service
{
    public partial interface IUserProfileService
    {
        Task<UserProfileModel> GetUserProfile(IActor actor);
    }
}