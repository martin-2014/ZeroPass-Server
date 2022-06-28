using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ZeroPass.Service;

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

        public static IServiceCollection UserMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            return services.AddSingleton(mapperConfig.CreateMapper());
        }
    }
}
