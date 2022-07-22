using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroPass.Model.Api;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;

namespace ZeroPass.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class VersionsController : ControllerAuthorizationBase
    {
        private readonly IClientVersionService ClientVersionService;

        public VersionsController(IClientVersionService clientVersionService) 
            => ClientVersionService = clientVersionService;

        [HttpGet("check")]
        [ApiResponseSuccess(typeof(ClientMinVersionCheckModel))]
        public async Task<IActionResult> CheckEmail()
        {
            var (meetMinVersion, minVersion) = await ClientVersionService.MeetMinimumVersion(new ClientVersionModel
                {Edition = Edition, Version = Version, DeviceId = DeviceId});
            
            return ApiResult.Success(new ClientMinVersionCheckModel{MeetMinVersion = meetMinVersion, MinVersion = minVersion});
        }
    }
}