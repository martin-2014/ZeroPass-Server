using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using ZeroPass.Model.Models;
using ZeroPass.Model.Models.UserProfiles;
using ZeroPass.Model.Service;

namespace ZeroPass.Api.Tests
{
    public partial class UsersProfileControllerTests : TestBase
    {
        const string Path = "/api/me/UserProfile";
        
        [Fact]
        public async Task GetUserWithProfile_Existed()
        {
            var user = TestEnv.Users.First();
            
            var tokenService = Application.GetRequiredService<ITokenService>();
            var token = await tokenService.Authenticate(user.Email);
            
            var request = RequestBuilder.GetRequest(Path).AddBearerToken(token).Build();
            var response = await Execute(request);
            Expect(response.IsSuccess).BeTrue();
            var fetchedUser = response.Body.Value<UserProfileModel>();
            Expect(fetchedUser.Id).Be(user.Id);
            Expect(fetchedUser.Email).Be(user.Email);
        }
    }
}