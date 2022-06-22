using System.Linq;
using System.Threading.Tasks;
using Xunit;

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
    }
}
