using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;

namespace ZeroPass.Api.Tests
{
    public class NotificationsControllerTests : TestBase
    {
        [Fact]
        public async Task ListActive_Success()
        {
            var user = TestEnv.Users.First();
            var tokenService = Application.GetRequiredService<ITokenService>();
            var token = await tokenService.Authenticate(user.Email);
            var request = RequestBuilder.GetRequest("api/notifications/active").AddBearerToken(token).Build();
            var response = await Execute(request);
            Expect(response.IsSuccess).BeTrue();
            var notifications = response.Body.Value<IEnumerable<NotificationModel<JsonElement>>>();
            Expect(notifications).BeEmpty();
        }
    }
}