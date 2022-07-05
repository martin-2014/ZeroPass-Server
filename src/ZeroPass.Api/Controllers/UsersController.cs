using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZeroPass.Api.Properties;
using ZeroPass.Model.Api;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;

namespace ZeroPass.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public partial class UsersController : ControllerAuthorizationBase
    {
        readonly IUserService UserService;

        [HttpGet("registration/{email}")]
        [ApiResponseSuccess()]
        [ApiResponseError(nameof(Resources.ErrorEmailDuplicate))]
        public async Task<IActionResult> CheckEmail(string email)
        {
            if (await UserService.ExistsUserByEmail(email))
                return ApiResult.Error(Resources.ErrorEmailDuplicate);
            return ApiResult.Success();
        }

        [HttpPost("registration")]
        [ApiResponseSuccess()]
        [ApiResponseError(nameof(Resources.ErrorEmailDuplicate))]
        public async Task<IActionResult> RegisterUser(UserRegisterModel model)
        {
            if (await UserService.ExistsUserByEmail(model.Email)) 
                return ApiResult.Error(Resources.ErrorEmailDuplicate);

            await UserService.RegisterUser(model);
            return ApiResult.Success();
        }
    }
}
