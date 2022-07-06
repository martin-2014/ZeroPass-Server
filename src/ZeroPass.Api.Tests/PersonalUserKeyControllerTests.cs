using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ZeroPass.Model.Models;

namespace ZeroPass.Api.Tests
{
    public class PersonalUserKeyControllerTests : TestBase
    {
        const string DataKeyPath = "/api/me/userkey/datakey";

        [Fact]
        public async Task GetPersonalUserDataKey_Success()
        {
            var user = TestEnv.Users.First();
            var clientData = new TestUserClientData(user.UserSecret);
            var token = await PersonalLogin(clientData);

            var requestData = clientData.GetRequestData(DateTime.Now.ToString());
            var request = RequestBuilder.PostRequest(DataKeyPath).AddBearerToken(token).WithBody(requestData).Build();
            var response = await Execute(request);

            Expect(response.IsSuccess).BeTrue();
            var dataKeyModel = clientData.GetReponseData<DataKeyModel>(response.Body.Payload.ToString());
            var userKeyEntity = FakeDatabase.UserKeys.Single(k => k.UserId == user.Id);
            Expect(userKeyEntity.PrivateDataKey).Be(dataKeyModel.SelfPrivateKey);
            Expect(userKeyEntity.PublicDataKey).Be(dataKeyModel.PublicKey);
            Expect(userKeyEntity.UserId).Be(dataKeyModel.AssignerId);
        }

        [Fact]
        public async Task GetPersonalUserDataKey_WithUserLoginInOtherDevice_Success()
        {
            var user = TestEnv.Users.First();
            var clientData = new TestUserClientData(user.UserSecret);
            var token = await PersonalLogin(clientData, "device1");

            var clientDataOfDevice2 = new TestUserClientData(user.UserSecret);
            await PersonalLogin(clientDataOfDevice2, "device2");

            var requestData = clientData.GetRequestData(DateTime.Now.ToString());
            var request = RequestBuilder.PostRequest(DataKeyPath).AddBearerToken(token)
                .AddHeader("Device-Id", "device1").WithBody(requestData).Build();
            var response = await Execute(request);

            Expect(response.IsSuccess).BeTrue();
            var dataKeyModel = clientData.GetReponseData<DataKeyModel>(response.Body.Payload.ToString());
            var userKeyEntity = FakeDatabase.UserKeys.Single(k => k.UserId == user.Id);
            Expect(userKeyEntity.PrivateDataKey).Be(dataKeyModel.SelfPrivateKey);
            Expect(userKeyEntity.PublicDataKey).Be(dataKeyModel.PublicKey);
            Expect(userKeyEntity.UserId).Be(dataKeyModel.AssignerId);
        }
    }
}