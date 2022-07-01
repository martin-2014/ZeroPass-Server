using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZeroPass.Model.Logging;
using ZeroPass.Service;
using ZeroPass.Service.Configuration;
using ZeroPass.Service.Logging;
using ZeroPass.Storage;

namespace ZeroPass.Api
{
    public class Startup
    {
        const string APPSETTINGS_PATH = "appsettings.json";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            Model.Configuration.IConfiguration config = new EnvironmentConfig();
            IAuthorizationConfiguration authConfig = new AuthorizationConfiguration(config);
            services.AddControllers();
            services.UseSwagger();
            services.AddSingleton(config)
                .AddSingleton(authConfig)
                .UseStorage(config)
                .ConfigureDapper()
                .UseCaching(config)
                .UseAuthentication(authConfig)
                .UseApplicationServices()
                .UseCodeGenerator()
                .UseNotification()
                .UseDataSecurity()
                .UserMapper()
                .AddMediatR(typeof(Startup)); ;

            services.AddSingleton<ISharedLogger, SharedLogger>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseHttpsRedirection();
            app.UseAuthentication();
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            var configuration = new ConfigurationBuilder().AddJsonFile(APPSETTINGS_PATH).Build();
            loggerFactory.AddLambdaLogger(new LambdaLoggerOptions(configuration));

            app.UseRouting();

            app.UseAuthorization();
            app.UseMiddleware<RequestIdHandlerMiddleware>();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
                });
            });
        }
    }
}