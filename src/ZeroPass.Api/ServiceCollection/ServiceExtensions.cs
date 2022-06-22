using Microsoft.Extensions.DependencyInjection;

namespace ZeroPass.Api
{
    public static class ServiceExtensions
    {
        public static IServiceCollection UseSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
            });
        }
    }
}
