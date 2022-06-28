using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ZeroPass.Model.Models;

namespace ZeroPass.Api.Tests
{
    public class UsersControllerTests : TestBase
    {
        const string RegistrationPath = "/api/users/registration";

        [Fact]
        public async Task CheckEmail_Success()
        {
            var email = "user_not_exist@test.com";
            var request = RequestBuilder.GetRequest($"{RegistrationPath}/{email}").Build();
            var response = await Execute(request);

            Expect(response.IsSuccess).BeTrue();
        }

        [Fact]
        public async Task CheckEmail_EmailDuplicate_DuplicateError()
        {
            var email = TestEnv.Users.FirstOrDefault().Email;
            var request = RequestBuilder.GetRequest($"{RegistrationPath}/{email}").Build();
            var response = await Execute(request);

            Expect(response.IsSuccess).BeFalse();
            Expect(response.Body.Error.Id).Be("err_email_duplicate");
        }

        [Fact]
        public async Task RegisterUser_EmailDuplicated_DuplicationError()
        {
            var model = CreateUserRegisterModel();
            model.Email = TestEnv.Users.FirstOrDefault().Email;
            var request = RequestBuilder.PostRequest(RegistrationPath).WithBody(model).Build();
            var response = await Execute(request);
            Expect(response.IsSuccess).BeFalse();
            Expect(response.Body.Error.Id).Be("err_email_duplicate");
        }

        [Fact]
        public async Task RegisterUser_Success()
        {
            var model = CreateUserRegisterModel();
            var request = RequestBuilder.PostRequest(RegistrationPath).WithBody(model).Build();
            var response = await Execute(request);
            Expect(response.IsSuccess).BeTrue();
            Expect(EmailServiceFake.Recipients).ContainSingle(r => r == model.Email);
            var key = CacheKeyGenerator.GenerateActivationKey(model.Email);
            Expect(CacheFake.Values.ContainsKey(key)).BeTrue();
        }

        static UserRegisterModel CreateUserRegisterModel(
            UserType accountType = UserType.Standard,
            string email = "e@e.com"
            )
            => new UserRegisterModel
            {
                AccountType = accountType,
                Email = email,
                Timezone = "t",
            };
    }
}
