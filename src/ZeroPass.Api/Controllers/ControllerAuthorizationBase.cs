using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ZeroPass.Model.Models;
using ZeroPass.Service;

namespace ZeroPass.Api
{
    public abstract class ControllerAuthorizationBase : ControllerBase
    {
        protected TokenModel Token => TokenService.GetToken(User);
        protected string DeviceId => GetHeaderValue("Device-Id");
        protected string Edition => GetHeaderValue("Edition");
        protected string Version => GetHeaderValue("Version");

        private string GetHeaderValue(string key)
        {
            var value = string.Empty;
            
            if (Request.Headers.TryGetValue(key, out var headerValues))
            {
                value = headerValues.FirstOrDefault();
            }

            return value;
        }
    }
}
