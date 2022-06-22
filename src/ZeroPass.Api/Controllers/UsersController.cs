using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZeroPass.Model.Api;
using ZeroPass.Model.Service;

namespace ZeroPass.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerAuthorizationBase
    {
        readonly IUserService UserService;

        public UsersController(IUserService userService)
        {
            UserService = userService;
        }

        [HttpGet("registration/{email}")]
        [ApiResponseSuccess()]
        [ApiResponseError(nameof(Resources.ErrorEmailDuplicate))]
        public async Task<IActionResult> CheckEmail(string email)
        {
            if (await UserService.ExistsUserByEmail(email))
                return ApiResult.Error(Resources.ErrorEmailDuplicate);
            return ApiResult.Success();
        }
    }
}
