using Amazon.Lambda.AspNetCoreServer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.AspNetCoreServer.Internal;
using Amazon.Lambda.Core;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.Extensions.Hosting;
using ZeroPass.Fakes;
using ZeroPass.Model.Configuration;
using ZeroPass.Model.Logging;
using ZeroPass.Service;

namespace ZeroPass.Api.Tests
{
    public partial class TestEntryPoint : APIGatewayProxyFunction
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

                services.AddMediatR(Assembly.GetAssembly(typeof(ZeroPass.Service.NotificationService)));
                IAuthorizationConfiguration authConfig = new AuthorizationConfiguration(Configuration);
                services = services
                    .UseAuthentication(authConfig)
                    .AddSingleton<IConfiguration>(Configuration)
                    .AddSingleton(authConfig)
                    .AddTransient<ISharedLogger, LoggerFake>();

                AddService(services);
            }).Configure(app =>
            {
                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseDeveloperExceptionPage();
                app.UseRouting();
                app.UseAuthorization();
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
        #region Hide the virtual methods in sub classes

        sealed protected override void PostMarshallConnectionFeature(IHttpConnectionFeature aspNetCoreConnectionFeature, APIGatewayProxyRequest lambdaRequest, ILambdaContext lambdaContext)
        {
            base.PostMarshallConnectionFeature(aspNetCoreConnectionFeature, lambdaRequest, lambdaContext);
        }

        sealed protected override IHostBuilder CreateHostBuilder()
        {
            return base.CreateHostBuilder();
        }

        sealed protected override void Init(IHostBuilder builder)
        {
            base.Init(builder);
        }

        [System.Obsolete]
        sealed protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return base.CreateWebHostBuilder();
        }

        sealed protected override void MarshallRequest(InvokeFeatures features, APIGatewayProxyRequest apiGatewayRequest, ILambdaContext lambdaContext)
        {
            base.MarshallRequest(features, apiGatewayRequest, lambdaContext);
        }

        sealed protected override APIGatewayProxyResponse MarshallResponse(IHttpResponseFeature responseFeatures, ILambdaContext lambdaContext, int statusCodeIfNotSet = 200)
        {
            return base.MarshallResponse(responseFeatures, lambdaContext, statusCodeIfNotSet);
        }

        sealed protected override void PostCreateHost(IHost webHost)
        {
            base.PostCreateHost(webHost);
        }

        sealed protected override void PostCreateWebHost(IWebHost webHost)
        {
            base.PostCreateWebHost(webHost);
            Application = webHost.Services;
        }

        sealed protected override void PostMarshallHttpAuthenticationFeature(IHttpAuthenticationFeature aspNetCoreHttpAuthenticationFeature, APIGatewayProxyRequest lambdaRequest, ILambdaContext lambdaContext)
        {
            base.PostMarshallHttpAuthenticationFeature(aspNetCoreHttpAuthenticationFeature, lambdaRequest, lambdaContext);
        }

        sealed protected override void PostMarshallItemsFeatureFeature(IItemsFeature aspNetCoreItemFeature, APIGatewayProxyRequest lambdaRequest, ILambdaContext lambdaContext)
        {
            base.PostMarshallItemsFeatureFeature(aspNetCoreItemFeature, lambdaRequest, lambdaContext);
        }

        sealed protected override void PostMarshallRequestFeature(IHttpRequestFeature aspNetCoreRequestFeature, APIGatewayProxyRequest lambdaRequest, ILambdaContext lambdaContext)
        {
            base.PostMarshallRequestFeature(aspNetCoreRequestFeature, lambdaRequest, lambdaContext);
        }

        sealed protected override void PostMarshallResponseFeature(IHttpResponseFeature aspNetCoreResponseFeature, APIGatewayProxyResponse lambdaResponse, ILambdaContext lambdaContext)
        {
            base.PostMarshallResponseFeature(aspNetCoreResponseFeature, lambdaResponse, lambdaContext);
        }

        sealed protected override void PostMarshallTlsConnectionFeature(ITlsConnectionFeature aspNetCoreConnectionFeature, APIGatewayProxyRequest lambdaRequest, ILambdaContext lambdaContext)
        {
            base.PostMarshallTlsConnectionFeature(aspNetCoreConnectionFeature, lambdaRequest, lambdaContext);
        }

        #endregion
    }
}
