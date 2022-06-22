using Amazon.Lambda.AspNetCoreServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using ZeroPass.Fakes;
using ZeroPass.Model.Configuration;
using ZeroPass.Model.Logging;

namespace ZeroPass.Api.Tests
{
    public class TestEntryPoint : APIGatewayProxyFunction
    {
        protected IServiceProvider Application;
        protected ConfigurationFake Configuration = new ConfigurationFake();

        protected virtual void AddService(IServiceCollection services) { }

        sealed protected override void Init(IWebHostBuilder builder)
        {
            InitConfiguration();
            builder.ConfigureServices(services =>
            {
                services.AddMvc()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    })
                    .AddControllersAsServices()
                    .AddApplicationPart(typeof(Startup).Assembly);

                services = services
                    .AddSingleton<IConfiguration>(Configuration)
                    .AddTransient<ISharedLogger, LoggerFake>();

                AddService(services);
            }).Configure(app =>
            {
                app.UseHttpsRedirection();
                app.UseDeveloperExceptionPage();
                app.UseRouting();

                app.UseMiddleware<RequestIdHandlerMiddleware>();

                app.UseMiddleware<ErrorHandlerMiddleware>();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            });
        }

        void InitConfiguration()
        {
            Configuration.SetValue("JWT_SECURITY_KEY", " 0r%SU5px6ZQKapoiy0w*8CevEC%N/}");
            Configuration.SetValue("JWT_EXPIRES_SECONDS", "1800");
        }

    }
}
