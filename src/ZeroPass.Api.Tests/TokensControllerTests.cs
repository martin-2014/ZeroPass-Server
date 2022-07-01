using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;
using ZeroPass.Storage.Fakes;

namespace ZeroPass.Api.Tests
{
    public partial class TokensControllerTests : TestBase
    {
        const string TokensPath = "/api/tokens";
        const string PublicKeyPath = "/api/userkey/publickey";
        
        [Fact]
        public async Task Login_WithGeneralUser_Success()
        {
            var user = TestEnv.Users.First();
            var clientData = new TestUserClientData(user.UserSecret);
            var publicKeyModel = clientData.GetClientPublicKey();

            var request = RequestBuilder.PostRequest(PublicKeyPath).WithBody(publicKeyModel).Build();
            var response = await Execute(request);
            Expect(response.IsSuccess).BeTrue();
            var userPublicKey = response.Body.Value<UserPublicKeyModel>();

            var authenticateModel = clientData.GetAuthenticateModel(userPublicKey);
            request = RequestBuilder.PostRequest(TokensPath).WithBody(authenticateModel).Build();
            response = await Execute(request);
            Expect(response.IsSuccess).BeTrue();
            Expect(response.Body.Value<JwtToken>().Token).NotBeNullOrEmpty();
        }
        
        [Fact]
        public async Task Login_WithWrongEmail_Failure()
        {
            var user = TestEnv.Users.First();
            var userSecret = new TestUserSecret()
            {
                UserId = user.UserSecret.UserId,
                Password = user.UserSecret.Password,
                SecretKey = user.UserSecret.SecretKey,
                Email = "wrong@abc.com"
            };
            var clientData = new TestUserClientData(userSecret);
            var publicKeyModel = clientData.GetClientPublicKey();

            var request = RequestBuilder.PostRequest(PublicKeyPath).WithBody(publicKeyModel).Build();
            var response = await Execute(request);
            Expect(response.IsSuccess).BeFalse();
            Expect(response.Body.Error.Id).Be("err_authentication_failed");
        }
        
        [Fact]
        public async Task Login_WithWrongPassword_Failure()
        {
            var user = TestEnv.Users.First();
            var userSecret = new TestUserSecret()
            {
                UserId = user.UserSecret.UserId,
                Password = "wrong",
                SecretKey = user.UserSecret.SecretKey,
                Email = user.UserSecret.Email
            };
            var clientData = new TestUserClientData(userSecret);
            var publicKeyModel = clientData.GetClientPublicKey();

            var request = RequestBuilder.PostRequest(PublicKeyPath).WithBody(publicKeyModel).Build();
            var response = await Execute(request);
            Expect(response.IsSuccess).BeTrue();
            var userPublicKey = response.Body.Value<UserPublicKeyModel>();

            var authenticateModel = clientData.GetAuthenticateModel(userPublicKey);
            request = RequestBuilder.PostRequest(TokensPath).WithBody(authenticateModel).Build();
            response = await Execute(request);
            Expect(response.IsSuccess).BeFalse();
            Expect(response.Body.Error.Id).Be("err_authentication_failed");
        }
        
        [Fact]
        public async Task Login_WithWrongSecretKey_Failure()
        {
            var user = TestEnv.Users.First();
            var userSecret = new TestUserSecret()
            {
                UserId = user.UserSecret.UserId,
                Password = user.UserSecret.Password,
                SecretKey = "AEE33C1F57D69AAD9E06B42D2D2DFFCB",
                Email = user.UserSecret.Email
            };
            var clientData = new TestUserClientData(userSecret);
            var publicKeyModel = clientData.GetClientPublicKey();

            var request = RequestBuilder.PostRequest(PublicKeyPath).WithBody(publicKeyModel).Build();
            var response = await Execute(request);
            Expect(response.IsSuccess).BeTrue();
            var userPublicKey = response.Body.Value<UserPublicKeyModel>();

            var authenticateModel = clientData.GetAuthenticateModel(userPublicKey);
            request = RequestBuilder.PostRequest(TokensPath).WithBody(authenticateModel).Build();
            response = await Execute(request);
            Expect(response.IsSuccess).BeFalse();
            Expect(response.Body.Error.Id).Be("err_authentication_failed");
        }
        
        [Fact]
        public async Task RefreshToken_Success()
        {
            var user = TestEnv.Users.First();
            var tokenService = Application.GetRequiredService<ITokenService>();
            var token = await tokenService.Authenticate(user.Email);
            Expect(token).NotBeNullOrEmpty();

            await Task.Delay(1000);

            var originalToken = token;
            var request = RequestBuilder.PutRequest(TokensPath).AddBearerToken(originalToken).Build();
            var responseRefreshToken = await Execute(request);
            Expect(responseRefreshToken.IsSuccess).BeTrue();
            Expect(responseRefreshToken.Body.Value<JwtToken>().Token).NotBeNullOrEmpty();

            Expect(originalToken).NotBeEquivalentTo(responseRefreshToken.Body.Value<JwtToken>().Token);
        }
        
        [Fact]
        public async Task RefreshToken_WhenExpired_Failure()
        {
            Configuration.SetValue("JWT_EXPIRES_SECONDS", "1");
            var user = TestEnv.Users.First();
            var tokenService = Application.GetRequiredService<ITokenService>();
            var token = await tokenService.Authenticate(user.Email);
            Expect(token).NotBeNullOrEmpty();

            await Task.Delay(1000);

            var originalToken = token;
            var request = RequestBuilder.PutRequest(TokensPath).AddBearerToken(originalToken).Build();
            var responseRefreshToken = await Execute(request);
            Expect(responseRefreshToken.IsSuccess).BeFalse();
            Expect(responseRefreshToken.StatusCode).Be((int)HttpStatusCode.Unauthorized);
        }
    }
}