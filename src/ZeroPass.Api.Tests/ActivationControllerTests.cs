using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using ZeroPass.Model.Models;
using ZeroPass.Storage;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Api.Tests
{
    public partial class ActivationControllerTests : TestBase
    {
        const string ActivationPath = "/api/activation";

        [Fact]
        public async Task CodeVerify_ModelValid_Success()
        {
            var model = CreateCodeVerifyModel();
            var key = CacheKeyGenerator.GenerateActivationKey(model.Email);
            await CacheFake
                .SetBytesWithAbsoluteExpiration(
                    key, 
                    new RegistrationEntity { 
                        Email = model.Email, 
                        Code = model.Code 
                    }.ToByteArray(), 0);

            var request = RequestBuilder.GetRequest(ActivationPath)
                .AddQueryParameter("email", model.Email)
                .AddQueryParameter("code", model.Code).Build();
            var response = await Execute(request);
            Expect(response.IsSuccess).BeTrue();
            var value = response.Body.Value<CodeVerifyResultModel>();
            Expect(value.Email).Be(model.Email);
            Expect(value.Code).Be(model.Code);
        }

        [Theory()]
        [MemberData(nameof(GetInvalidCodeVerifyModels))]
        public async Task CodeVerify_MissingRequiredFields_InvalidModelError(CodeVerifyModel model)
        {
            var request = RequestBuilder.GetRequest(ActivationPath)
               .AddQueryParameter("email", model.Email)
               .AddQueryParameter("code", model.Code).Build();
            var response = await Execute(request);
            Expect(response.StatusCode).Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CodeVerify_CodeNotMatch_InvalidCodeError()
        {
            var model = CreateCodeVerifyModel();
            var key = CacheKeyGenerator.GenerateActivationKey(model.Email + "!");
            await CacheFake.SetBytesWithAbsoluteExpiration(key, new RegistrationEntity().ToByteArray(), 0);

            var request = RequestBuilder.GetRequest(ActivationPath)
               .AddQueryParameter("email", model.Email)
               .AddQueryParameter("code", model.Code).Build();
            var response = await Execute(request);
            Expect(response.IsSuccess).BeFalse();
            Expect(response.Body.Error.Id).Be("err_invalid_code");
        }

        [Fact]
        public async Task ActivateAccount_InvalidCodeError()
        {
            var model = CreateActivateAccountModel();
            var request = RequestBuilder.PostRequest(ActivationPath)
                .WithBody(model).Build();
            var response = await Execute(request);
            Expect(response.IsSuccess).BeFalse();
            Expect(response.Body.Error.Id).Be("err_invalid_code");
        }

        [Fact]
        public async Task ActivateAccount_User_Success()
        {
            var model = CreateActivateAccountModel();
            var key = CacheKeyGenerator.GenerateActivationKey(model.Email);
            await CacheFake.SetBytesWithAbsoluteExpiration(key, new RegistrationEntity
            {
                Raw = new UserRegisterModel { Email = "a@a.com" },
                Code = model.Code,
                ActivateType = ActivateType.Personal
            }.ToByteArray(), 0);

            var request = RequestBuilder.PostRequest(ActivationPath)
                .WithBody(model).Build();
            var response = await Execute(request);
            Expect(response.IsSuccess).BeTrue();
        }

        static CodeVerifyModel CreateCodeVerifyModel(
            string email = "a@b.com",
            string code = "v")
            => new CodeVerifyModel
            {
                Email = email,
                Code = code
            };

        static ActivateAccountModel CreateActivateAccountModel(
           ActivateType type = ActivateType.Personal,
           string email = "e",
           string code = "c"
           )
           => new ActivateAccountModel
           {
               ActivateType = (int)type,
               Email = email,
               Code = code,
               UserKey = new UserKeyCreateModel
               {
                   Salt = "s",
                   Verifier = "v",
                   PublicDataKey = "p",
                   PrivateDataKey = "p"
               }
           };

        public static IEnumerable<object[]> GetInvalidCodeVerifyModels()
        {
            yield return new object[] { CreateCodeVerifyModel(email: null) };
            yield return new object[] { CreateCodeVerifyModel(email: "") };
            yield return new object[] { CreateCodeVerifyModel(email: " ") };
            yield return new object[] { CreateCodeVerifyModel(code: null) };
            yield return new object[] { CreateCodeVerifyModel(code: "") };
            yield return new object[] { CreateCodeVerifyModel(code: " ") };
        }
    }
}
