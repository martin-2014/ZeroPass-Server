using Microsoft.Extensions.DependencyInjection;
using ZeroPass.Model.Configuration;

namespace ZeroPass.Storage
{
    public static class StorageExtensions
    {
        public static IServiceCollection UseStorage(this IServiceCollection services, IConfiguration config)
        {
            var writableConnectionString = config.GetValue("MysqlConnectionString");
            var readonlyConnectionString = config.GetValue("ReadonlyMysqlConnectionString");
            var connectionOptions = new ConnectionOption()
            {
                MasterMysqlConnectionString = writableConnectionString,
                ReadonlyMysqlConnectionString = readonlyConnectionString
            };
            return services
                .AddSingleton(connectionOptions)
                .UseUnitOfWork();
        }

        public static IServiceCollection UseUnitOfWork(this IServiceCollection services)
            => services
                .AddSingleton<IDomainDataState, DomainDataState>()
                .AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();

        public static IServiceCollection UseCaching(this IServiceCollection services, IConfiguration config) 
            => services
                .AddSingleton<ICache, RedisCache>()
                .AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = config.GetValue("RedisConnectionString");
                });
    }
}
