using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ZeroPass.Service;

namespace ZeroPass.Api
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection UseAuthentication(this IServiceCollection services, IAuthorizationConfiguration authConfig)
        {
            services.AddAuthorization(options =>
                {
                    foreach(var policyRoles in authConfig.Policies)
                    {
                        options.AddPolicy(policyRoles.Key, policy =>
                            policy.Requirements.Add(new PolicyRoleRequirement() { AllowedRoles = policyRoles.Value.ToList() }));
                    }
                })
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = authConfig.TokenValidationParameters;
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            //Token expired
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            return services.AddTransient<IAuthorizationHandler, PolicyRoleHandler>();
        }

    }
}