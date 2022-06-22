using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using FluentAssertions;
using FluentAssertions.Primitives;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using ZeroPass.Service;
using ZeroPass.Storage;
using ZeroPass.Storage.Fakes;

namespace ZeroPass.Api.Tests
{
    public class TestBase : TestEntryPoint
    {
        UserRepositoryFake userRepositoryFake;
        protected UserRepositoryFake UserRepositoryFake => userRepositoryFake;
        protected readonly FakeDatabase FakeDatabase = new FakeDatabase();
        protected TestEnvironment TestEnv => new TestEnvironment(FakeDatabase);
        protected TestEnvironmentBuilder EnvBuilder => new TestEnvironmentBuilder(TestEnv);

        public TestBase() => SetupEnv();

        void SetupEnv()
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
                .AddSingleton<IUserRepository>(UserRepositoryFake)
                .AddSingleton<IUnitOfWorkFactory>(new UnitOfWorkFactoryFake(FakeDatabase));
        }

        void InitRepositories()
        {
            userRepositoryFake = new UserRepositoryFake(FakeDatabase);
        }

        protected async Task<TestResponse> Execute(APIGatewayProxyRequest request)
        {
            var reponse = await FunctionHandlerAsync(request, new TestLambdaContext());
            return new TestResponse(reponse);
        }

        protected BooleanAssertions Expect(bool value) => value.Should();

        protected StringAssertions Expect(string value) => value.Should();
    }
}
