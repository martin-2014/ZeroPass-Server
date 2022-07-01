using Microsoft.AspNetCore.Mvc;
using ZeroPass.Model.Models;
using ZeroPass.Service;

namespace ZeroPass.Api
{
    public abstract class ControllerAuthorizationBase : ControllerBase
    {
        protected TokenModel Token => TokenService.GetToken(User);
    }
}
