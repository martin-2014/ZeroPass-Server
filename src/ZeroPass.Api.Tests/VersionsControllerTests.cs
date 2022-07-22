using System.Threading.Tasks;
using Xunit;
using ZeroPass.Model.Models;

namespace ZeroPass.Api.Tests
{
    public class VersionsControllerTests : TestBase
    {
        private const string VersionCheckPath = "/api/versions/check";

        [Theory]
        [InlineData("1.0.1", true)]
        [InlineData("0.0.1", false)]
        public async Task VersionsCheck_Success(string clientVersion, bool meet)
        {
            var request = RequestBuilder
                .GetRequest(VersionCheckPath)
                .AddHeader("Edition", "community")
                .AddHeader("Version", clientVersion)
                .Build();
            var expectedResult = new ClientMinVersionCheckModel
            {
                MeetMinVersion = meet,
                MinVersion = "1.0.0"
            };
            var response = await Execute(request);
            Expect(response.IsSuccess).BeTrue();
            Expect(response.Body.Value<ClientMinVersionCheckModel>()).BeEquivalentTo(expectedResult);
        }
        
        [Fact]
        public async Task VersionsCheck_Success_With_NoClientInfo()
        {
            var request = RequestBuilder
                .GetRequest(VersionCheckPath)
                .Build();
            var expectedResult = new ClientMinVersionCheckModel
            {
                MeetMinVersion = false,
                MinVersion = string.Empty
            };
            var response = await Execute(request);
            Expect(response.IsSuccess).BeTrue();
            Expect(response.Body.Value<ClientMinVersionCheckModel>()).BeEquivalentTo(expectedResult);
        }
    }
}