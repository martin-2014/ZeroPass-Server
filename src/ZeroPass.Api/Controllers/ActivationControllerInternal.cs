using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroPass.Api.Properties;
using ZeroPass.Model.Api;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;

namespace ZeroPass.Api
{
    public partial class ActivationController
    {
        public ActivationController(IActivationService activationService) => ActivationService = activationService;

        [HttpPost()]
        [ApiResponseSuccess()]
        [ApiResponseError(nameof(Resources.ErrorInvalidCode), nameof(Resources.ErrorActivationFailed))]
        public async Task<IActionResult> ActivateAccount(ActivateAccountModel model)
        {
            var response = await ActivationService.CodeVerify(model);
            if (response == null) return ApiResult.Error(Resources.ErrorInvalidCode);

            var activatedAccount = await ActivationService.ActivateAccount(model);
            return activatedAccount == null ? 
                ApiResult.Error(Resources.ErrorActivationFailed) : 
                ApiResult.Success();
        }
    }
}