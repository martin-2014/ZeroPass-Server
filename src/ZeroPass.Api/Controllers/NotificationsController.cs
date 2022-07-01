using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeroPass.Model.Api;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;

namespace ZeroPass.Api
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerAuthorizationBase
    {
        readonly INotificationService NotificationService;

        public NotificationsController(INotificationService notificationService)
        {
            NotificationService = notificationService;
        }

        [HttpGet("active")]
        [ApiResponseSuccess(typeof(IEnumerable<NotificationModel<JsonElement>>))]
        public async Task<IActionResult> ListActive()
        {
            var actor = Token.Personal();
            var result = await NotificationService.ListActive<JsonElement>(actor.UserId);
            return ApiResult.Success(result);
        }

        [HttpPost("process")]
        [ApiResponseSuccess()]
        public async Task<IActionResult> Process(NotificationActionResultModel value)
        {
            var actor = Token.Personal();
            await NotificationService.Process(actor.UserId, value);
            return ApiResult.Success();
        }

        [HttpPost("clear")]
        [ApiResponseSuccess()]
        public async Task<IActionResult> Clear(NotificationClearModel value)
        {
            var actor = Token.Personal();
            await NotificationService.Clear(actor.UserId, value.Ids);
            return ApiResult.Success();
        }
    }
}
