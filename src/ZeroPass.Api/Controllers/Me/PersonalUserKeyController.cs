using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeroPass.Api.Properties;
using ZeroPass.Model.Api;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;

namespace ZeroPass.Api
{
    [Route("api/me/userKey")]
    [ApiController]
    [Authorize()]
    public class PersonalUserKeyController : ControllerAuthorizationBase
    {
        readonly IUserKeyService UserKeyService;
        
        public PersonalUserKeyController(IUserKeyService userKeyService)
            => (UserKeyService) = (userKeyService);
        
        [HttpPost("datakey")]
        [ApiResponseSuccess(typeof(DataKeyModel))]
        [ApiResponseError(nameof(Resources.ErrorAuthenticationFailed), StatusCode = (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetUserDataKey(UserKeyRequestModel model)
        {
            var actor = Token.Personal();
            var result = await UserKeyService.GetDataKey(actor, model);
            return ApiResult.Success(result);
        }
    }
}