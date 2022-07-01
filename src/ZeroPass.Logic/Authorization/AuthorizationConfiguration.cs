using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ZeroPass.Model.Configuration;

namespace ZeroPass.Service
{
    public interface IAuthorizationConfiguration
    {
        public TokenValidationParameters TokenValidationParameters { get; }
        public Dictionary<string, string[]> Policies { get; }
    }
    public class AuthorizationConfiguration : IAuthorizationConfiguration
    {
        readonly IConfiguration Configuration;
        readonly Dictionary<string, string[]> policies = new Dictionary<string, string[]>();

        public AuthorizationConfiguration(IConfiguration config)
        {
            Configuration = config;

            policies.Add(PolicyRoleHandler.PolicyNameForDomainOwner, new string[] { PolicyRoleHandler.DomainOwnerRoleName });
            policies.Add(PolicyRoleHandler.PolicyNameForDomainAdmin, new string[] { PolicyRoleHandler.DomainAdminRoleName });
        }

        public TokenValidationParameters TokenValidationParameters
        {
            get
            {
                return new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue("JWT_SECURITY_KEY")))
                };
            }
        }

        public Dictionary<string, string[]> Policies
        {
            get
            {
                return policies;
            }
        }
    }
}