using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;
using ZeroPass.Storage.Entities;

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
        
        [Fact]
        public async Task UpdateUserProfile_Success()
        {
            var user = TestEnv.Users.First();
            var tokenService = Application.GetRequiredService<ITokenService>();
            var token = await tokenService.Authenticate(user.Email);
            var request = RequestBuilder.GetRequest(Path).AddBearerToken(token).Build();
            var response = await Execute(request);
            Expect(response.IsSuccess).BeTrue();
            var fetchedUser = response.Body.Value<UserProfileModel>();

            var updateRequest = new UserProfileUpdateModel
            {
                UserName = "UserNameUpdated",
                Timezone = "TimezoneUpdated",
                Photo = "PhotoUpdated"
            };
            request = RequestBuilder.PutRequest(Path).AddBearerToken(token).WithBody(updateRequest).Build();
            response = await Execute(request);
            Expect(response.IsSuccess).BeTrue();

            request = RequestBuilder.GetRequest(Path).AddBearerToken(token).Build();
            response = await Execute(request);
            fetchedUser = response.Body.Value<UserProfileModel>();
            Expect(fetchedUser.Timezone).Be(updateRequest.Timezone);
            Expect(fetchedUser.UserName).Be(updateRequest.UserName);
            Expect(fetchedUser.Domains.Single(d =>d.DomainType == (int)DomainType.Personal).Logo).Be(updateRequest.Photo);
        }
    }
}