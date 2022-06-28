using Microsoft.Extensions.DependencyInjection;
using ZeroPass.Model;
using ZeroPass.Model.Service;

namespace ZeroPass.Service
{
    public static class ServiceExtensions
    {
        public static IServiceCollection UseApplicationServices(this IServiceCollection services)
            => services
            .AddSingleton<ICacheKeyGenerator, CacheKeyGenerator>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IActivationService, ActivationService>()
            .AddScoped<IUserKeyInternalService, UserKeyInternalService>();

        public static IServiceCollection UseCodeGenerator(this IServiceCollection services)
            => services.AddScoped<IRandom, Randomizer>();

        public static IServiceCollection UseNotification(this IServiceCollection services)
            => services.AddScoped<IEmailService, EmailService>();
    }
}
