using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroPass.Api.Properties;
using ZeroPass.Model.Api;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;

namespace ZeroPass.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class UserKeyController : ControllerAuthorizationBase
    {
        private readonly IUserKeyService UserKeyService;

        public UserKeyController(IUserKeyService userKeyService)
        {
            UserKeyService = userKeyService;
        }

        [HttpPost("publickey")]
        [ApiResponseSuccess(typeof(UserPublicKeyModel))]
        [ApiResponseError(nameof(Resources.ErrorAuthenticationFailed), StatusCode = (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> ExchangePublicKey(UserExchangePublicKeyModel model)
        {
            var key = await UserKeyService.ExchangePublicKey(model);
            return key == null ?
                ApiResult.Error(Resources.ErrorAuthenticationFailed, HttpStatusCode.Unauthorized) :
                ApiResult.Success(key);
        }
    }
}