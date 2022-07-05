using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroPass.Api.Properties;
using ZeroPass.Model.Api;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;

namespace ZeroPass.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public partial class ActivationController : ControllerBase
    {
        readonly IActivationService ActivationService;


        [HttpGet()]
        [ApiResponseSuccess(typeof(CodeVerifyResultModel))]
        [ApiResponseError(nameof(Resources.ErrorInvalidCode))]
        public async Task<IActionResult> CodeVerify([FromQuery] CodeVerifyModel model)
        {
            var response = await ActivationService.CodeVerify(model);
            return response != null 
                ? ApiResult.Success(response) 
                : ApiResult.Error(Resources.ErrorInvalidCode);
        }
    }
}
