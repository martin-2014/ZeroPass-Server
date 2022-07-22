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
    [ApiController]
    [Route("api/[controller]")]
    public partial class TokensController : ControllerAuthorizationBase
    {
        private readonly IUserKeyService UserKeyService;
        private readonly ITokenService TokenService;
        private readonly IClientVersionService ClientVersionService;
        
        public TokensController(
            IUserKeyService userKeyService, 
            ITokenService tokenService, 
            IClientVersionService clientVersionService)
        {
            UserKeyService = userKeyService;
            TokenService = tokenService;
            ClientVersionService = clientVersionService;
        }

        [HttpPost()]
        [ApiResponseSuccess(typeof(TokenResponse))]
        [ApiResponseError(nameof(Resources.ErrorAuthenticationFailed), StatusCode = (int)HttpStatusCode.Unauthorized)]
        [ApiResponseError(nameof(Resources.ErrorMinimumVersionRequired), StatusCode = (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Authenticate(AuthenticateModel model)
        {
            var identifierProof = await UserKeyService.Authenticate(model, DeviceId);
            if (identifierProof == null)
                return ApiResult.Error(Resources.ErrorAuthenticationFailed, HttpStatusCode.Unauthorized);

            var token = await TokenService.Authenticate(model.Request.ClientIdentifierProof.Email);

            if (token == null)
            {
                return ApiResult.Error(Resources.ErrorAuthenticationFailed, HttpStatusCode.Unauthorized);
            }

            var clientVersion = new ClientVersionModel
            {
                Email = model.Request.ClientIdentifierProof.Email,
                Edition = Edition, 
                Version = Version,
                DeviceId = DeviceId
            };
            await ClientVersionService.SaveClientVersion(clientVersion);
            var (meetMinVersion, _) = await ClientVersionService.MeetMinimumVersion(clientVersion);

            return meetMinVersion switch
            {
                false when string.IsNullOrEmpty(Edition) || string.IsNullOrEmpty(Version) => ApiResult.Error(
                    "Your version of the App is not supported now. Please notice the auto-upgrade reminder to upgrade."),
                false => ApiResult.Error(Resources.ErrorMinimumVersionRequired),
                _ => ApiResult.Success(new TokenResponse {Token = token, IdentifierProof = identifierProof})
            };
        }
        
        [HttpPut()]
        [Authorize]
        [ApiResponseSuccess(typeof(TokenResponse))]
        [ApiResponseError(nameof(Resources.ErrorAuthenticationFailed), StatusCode = (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> RefreshToken()
        {
            await UserKeyService.ActiveSession(Token.UserId, DeviceId);

            var token = await TokenService.RefreshToken(Token);
            return token == null ?
                ApiResult.Error(Resources.ErrorAuthenticationFailed, HttpStatusCode.Unauthorized) : 
                ApiResult.Success(new TokenResponse { Token = token });
        }
    }
}