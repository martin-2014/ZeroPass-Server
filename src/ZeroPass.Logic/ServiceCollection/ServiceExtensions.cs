using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using ZeroPass.Model;
using ZeroPass.Model.Models;
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
            .AddScoped<IUserKeyInternalService, UserKeyInternalService>()
            .AddScoped<ITokenService, TokenService>()
            .AddScoped<IUserKeyService, UserKeyService>()
            .AddScoped<ISessionFactory, SessionFactory>()
            .AddScoped<IUserProfileService, UserProfileService>()
            .AddScoped<INotificationService, NotificationService>();

        public static IServiceCollection UseCodeGenerator(this IServiceCollection services)
            => services.AddScoped<IRandom, Randomizer>();

        public static IServiceCollection UseNotification(this IServiceCollection services)
            => services.AddScoped<IEmailService, EmailService>();

        public static IServiceCollection UseDataSecurity(this IServiceCollection services)
            => services
            .AddSingleton<IConvertService, ConvertService>()
            .AddSingleton<ICryptoService, CryptoService>();

        public static IServiceCollection ConfigureDapper(this IServiceCollection services)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            Dapper.SqlMapper.AddTypeHandler(typeof(JsonElement), new JsonTypeHandler());

            GetJsonColumnTypes().ToList().ForEach(t => Dapper.SqlMapper.AddTypeHandler(t, new JsonTypeHandler()));

            return services;
        }

        static IEnumerable<Type>  GetJsonColumnTypes()
        {
            return typeof(JsonColumn).Assembly.GetTypes().Where(t => t.GetCustomAttribute<JsonColumn>() != null);
        }

        public class JsonTypeHandler : Dapper.SqlMapper.ITypeHandler
        {
            static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };


            public object Parse(Type destinationType, object value)
            {
                return JsonSerializer.Deserialize(value.ToString(), destinationType, JsonOptions);
            }

            public void SetValue(IDbDataParameter parameter, object value)
            {
                parameter.Value = (value == null) ? (object)DBNull.Value : JsonSerializer.Serialize(value, JsonOptions);
                parameter.DbType = DbType.String;
            }
        }

    }
}
