using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeroPass.Api.Properties;
using ZeroPass.Model.Api;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;

namespace ZeroPass.Api
{
    [Authorize]
    [ApiController]
    [Route("api/me/[controller]")]
    public class UserProfileController : ControllerAuthorizationBase
    {
        readonly IUserProfileService Service;

        public UserProfileController(IUserProfileService service)
            => Service = service;

        [HttpGet]
        [ApiResponseSuccess(typeof(UserProfileModel))]
        [ApiResponseError(nameof(Resources.ErrorNotFound))]
        public async Task<IActionResult> GetUserProfile()
        {
            var user = await Service.GetUserProfile(Token);
            return user != null ? ApiResult.Success(user) : ApiResult.Error(Resources.ErrorNotFound);
        }
        
        [HttpPut]
        [ApiResponseSuccess()]
        public async Task<IActionResult> Update(UserProfileUpdateModel request)
        {
            var actor = Token.Personal();
            await Service.UpdateUserProfile(actor, request);
            return ApiResult.Success();
        }
    }
}