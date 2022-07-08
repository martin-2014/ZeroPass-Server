using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using AutoMapper;
using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Numeric;
using FluentAssertions.Primitives;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroPass.Fakes;
using ZeroPass.Model;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;
using ZeroPass.Service;
using ZeroPass.Storage;
using ZeroPass.Storage.Fakes;

namespace ZeroPass.Api.Tests
{
    public partial class TestBase : TestEntryPoint
    {
        UserRepositoryFake userRepositoryFake;
        protected UserRepositoryFake UserRepositoryFake => userRepositoryFake;

        protected readonly FakeDatabase FakeDatabase = new FakeDatabase();
        protected TestEnvironment TestEnv => new TestEnvironment(FakeDatabase);
        protected TestEnvironmentBuilder EnvBuilder => new TestEnvironmentBuilder(TestEnv);

        protected readonly RandomFake CodeGeneratorFake = new RandomFake();
        protected readonly EmailServiceFake EmailServiceFake = new EmailServiceFake();
        protected readonly CacheFake CacheFake = new CacheFake();
        protected readonly ICacheKeyGenerator CacheKeyGenerator = new CacheKeyGenerator();

        UserProfileRepositoryFake userProfileRepositoryFake;
        protected UserProfileRepositoryFake UserProfileRepositoryFake => userProfileRepositoryFake;

        protected UnitOfWorkFactoryFake UnitOfWorkFactoryFake;

        IMapper mapper;
        protected IMapper Mapper => mapper;


        public TestBase() => SetupEnv();

        protected virtual void SetupEnv()
        {
            var userCount = 6;
            EnvBuilder.CreatePersonalUsers(userCount);
        }

        protected override void AddService(IServiceCollection services)
        {
            base.AddService(services);
            InitRepositories();
            services
                .UseApplicationServices()
                .UseDataSecurity()
                .AddSingleton<IUserRepository>(UserRepositoryFake)
                .AddSingleton<IUnitOfWorkFactory>(UnitOfWorkFactoryFake)
                .AddSingleton<IEmailService>(EmailServiceFake)
                .AddSingleton<IRandom>(CodeGeneratorFake)
                .AddSingleton<ICache>(CacheFake)
                .AddSingleton<IUserProfileRepository>(UserProfileRepositoryFake)
                .AddSingleton(CacheKeyGenerator)
                .AddSingleton(mapper); ;
        }

        protected virtual void InitRepositories()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            mapper = mapperConfig.CreateMapper();
            userRepositoryFake = new UserRepositoryFake(FakeDatabase);
            userProfileRepositoryFake = new UserProfileRepositoryFake(FakeDatabase);
            UnitOfWorkFactoryFake = new UnitOfWorkFactoryFake(FakeDatabase);
        }

        protected async Task<TestResponse> Execute(APIGatewayProxyRequest request)
        {
            var response = await FunctionHandlerAsync(request, new TestLambdaContext());
            return new TestResponse(response);
        }

        protected async Task<string> PersonalLogin(TestUserClientData userClientData, string deviceId = "")
        {
            var publicKeyModel = userClientData.GetClientPublicKey();
            var request = RequestBuilder.PostRequest("/api/userkey/publickey").WithBody(publicKeyModel).Build();
            var response = await Execute(request);
            Expect(response.IsSuccess).BeTrue();

            var userPublicKey = response.Body.Value<UserPublicKeyModel>();
            var authenticateModel = userClientData.GetAuthenticateModel(userPublicKey);
            request = RequestBuilder.PostRequest("/api/tokens")
                .AddHeader("Device-Id", deviceId).WithBody(authenticateModel).Build();
            response = await Execute(request);
            Expect(response.IsSuccess).BeTrue();
            return response.Body.Value<JwtToken>().Token;
        }

        protected BooleanAssertions Expect(bool value) => value.Should();

        protected StringAssertions Expect(string value) => value.Should();

        protected GenericCollectionAssertions<T> Expect<T>(IEnumerable<T> value) => value.Should();

        protected NumericAssertions<int> Expect(int value) => value.Should();
        
        protected ObjectAssertions Expect(object value) => value.Should();
        
        public class JwtToken
        {
            public string Token { get; set; }
        }
    }
}
