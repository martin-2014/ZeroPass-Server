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
        
        public TokensController(IUserKeyService userKeyService, ITokenService tokenService)
        {
            UserKeyService = userKeyService;
            TokenService = tokenService;
        }

        [HttpPost()]
        [ApiResponseSuccess(typeof(TokenResponse))]
        [ApiResponseError(nameof(Resources.ErrorAuthenticationFailed), StatusCode = (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Authenticate(AuthenticateModel model)
        {
            var identifierProof = await UserKeyService.Authenticate(model);
            if (identifierProof == null)
                return ApiResult.Error(Resources.ErrorAuthenticationFailed, HttpStatusCode.Unauthorized);

            var token = await TokenService.Authenticate(model.Request.ClientIdentifierProof.Email);
            return token == null ? 
                ApiResult.Error(Resources.ErrorAuthenticationFailed, HttpStatusCode.Unauthorized) : 
                ApiResult.Success(new TokenResponse { Token = token, IdentifierProof = identifierProof });
        }
        
        [HttpPut()]
        [Authorize]
        [ApiResponseSuccess(typeof(TokenResponse))]
        [ApiResponseError(nameof(Resources.ErrorAuthenticationFailed), StatusCode = (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> RefreshToken()
        {
            await UserKeyService.ActiveSession(Token.UserId);

            var token = await TokenService.RefreshToken(Token);
            return token == null ?
                ApiResult.Error(Resources.ErrorAuthenticationFailed, HttpStatusCode.Unauthorized) : 
                ApiResult.Success(new TokenResponse { Token = token });
        }
    }
}