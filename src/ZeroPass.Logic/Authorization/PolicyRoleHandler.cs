using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using ZeroPass.Storage;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Service
{
    public partial class PolicyRoleHandler : AuthorizationHandler<PolicyRoleRequirement>
    {
        public const string PolicyNameForDomainOwner = "ZeroPass.Policy.DomainOwner";
        public const string PolicyNameForDomainAdmin = "ZeroPass.Policy.DomainAdmin";

        public const string DomainOwnerRoleName = "DomainOwner";
        public const string DomainAdminRoleName = "DomainAdmin";

        readonly IHttpContextAccessor HttpContextAccessor;

        public PolicyRoleHandler(IHttpContextAccessor httpContextAccessor)
            => HttpContextAccessor = httpContextAccessor;

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRoleRequirement requirement)
        {
            var routeData = HttpContextAccessor.HttpContext.Request.Body;

            if (context.User == null || !context.User.Identity.IsAuthenticated || !ValidateClaims(context.User.Claims))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
        
        bool IsRequirementSatisfied(PolicyRoleRequirement requirement, DomainUserEntity user)
        {
            if (requirement.AllowedRoles == null || !requirement.AllowedRoles.Any())
                return true;

            return requirement.AllowedRoles.Any(role => IsValidRole(role, user));
        }

        bool IsValidRole(string requirementRole, DomainUserEntity user)
            => requirementRole switch
            {
                DomainOwnerRoleName when user.IsOwner => true,
                DomainAdminRoleName when user.IsAdmin => true,
                _ => false
            };
    }
}