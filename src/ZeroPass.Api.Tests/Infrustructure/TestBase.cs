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
using ZeroPass.Model.Service;
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

        protected readonly RandomFake CodeGeneratorFake = new RandomFake();
        protected readonly EmailServiceFake EmailServiceFake = new EmailServiceFake();
        protected readonly CacheFake CacheFake = new CacheFake();
        protected readonly ICacheKeyGenerator CacheKeyGenerator = new CacheKeyGenerator();

        IMapper mapper;
        protected IMapper Mapper => mapper;


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
                .AddSingleton<IUnitOfWorkFactory>(new UnitOfWorkFactoryFake(FakeDatabase))
                .AddSingleton<IEmailService>(EmailServiceFake)
                .AddSingleton<IRandom>(CodeGeneratorFake)
                .AddSingleton<ICache>(CacheFake)
                .AddSingleton(CacheKeyGenerator)
                .AddSingleton(mapper); ;
        }

        void InitRepositories()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            mapper = mapperConfig.CreateMapper();
            userRepositoryFake = new UserRepositoryFake(FakeDatabase);
        }

        protected async Task<TestResponse> Execute(APIGatewayProxyRequest request)
        {
            var reponse = await FunctionHandlerAsync(request, new TestLambdaContext());
            return new TestResponse(reponse);
        }

        protected BooleanAssertions Expect(bool value) => value.Should();

        protected StringAssertions Expect(string value) => value.Should();

        protected GenericCollectionAssertions<T> Expect<T>(IEnumerable<T> value) => value.Should();

        protected NumericAssertions<int> Expect(int value) => value.Should();
    }
}
