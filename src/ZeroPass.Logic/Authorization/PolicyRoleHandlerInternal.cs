using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace ZeroPass.Service
{
    public partial class PolicyRoleHandler
    {
        readonly IHttpContextAccessor HttpContextAccessor;

        public PolicyRoleHandler(IHttpContextAccessor httpContextAccessor)
            => HttpContextAccessor = httpContextAccessor;
        
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRoleRequirement requirement)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated || !ValidateClaims(context.User.Claims))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
        
        bool ValidateClaims(IEnumerable<Claim> claims)
        {
            if (claims.FirstOrDefault(c => c.Type == TokenService.UserIdClaimType) == null ||
                claims.FirstOrDefault(c => c.Type == TokenService.PersonalDomainIdClaimType) == null)
            {
                return false;
            }
            return true;
        }
    }
}