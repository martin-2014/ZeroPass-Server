using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZeroPass.Api.Properties;
using ZeroPass.Model.Api;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;
using ZeroPass.Service.Mediator;

namespace ZeroPass.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivationController : ControllerBase
    {
        readonly IActivationService ActivationService;
        readonly IMediator Mediator;

        public ActivationController(IActivationService activationService, IMediator mediator)
        {
            ActivationService = activationService;
            Mediator = mediator;
        }

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

        [HttpPost()]
        [ApiResponseSuccess()]
        [ApiResponseError(nameof(Resources.ErrorInvalidCode), nameof(Resources.ErrorActivationFailed))]
        public async Task<IActionResult> ActivateAccount(ActivateAccountModel model)
        {
            var response = await ActivationService.CodeVerify(model);
            if (response == null) return ApiResult.Error(Resources.ErrorInvalidCode);

            var activatedAccount = await ActivationService.ActivateAccount(model);
            if (activatedAccount != null)
            {
                if (!activatedAccount.IsBusiness)
                {
                    await Mediator.Publish(new UserRegisteredEvent(activatedAccount.UserId, model.Email));
                }
                return ApiResult.Success();
            }

            return ApiResult.Error(Resources.ErrorActivationFailed);
        }
    }
}
