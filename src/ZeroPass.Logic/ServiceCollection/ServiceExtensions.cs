using Microsoft.Extensions.DependencyInjection;
using ZeroPass.Model.Service;

namespace ZeroPass.Service
{
    public static class ServiceExtensions
    {
        public static IServiceCollection UseApplicationServices(this IServiceCollection services)
            => services
            .AddScoped<IUserService, UserService>();
    }
}
