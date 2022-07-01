using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ZeroPass.Service
{
    public partial class PolicyRoleHandler
    {
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