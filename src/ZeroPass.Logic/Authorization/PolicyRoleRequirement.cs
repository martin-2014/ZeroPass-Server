using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace ZeroPass.Service
{
    public class PolicyRoleRequirement : IAuthorizationRequirement
    {
        public PolicyRoleRequirement()
            => AllowedRoles = new List<string>();
        
        public List<string> AllowedRoles { get; set; }
    }
}