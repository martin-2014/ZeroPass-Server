using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ZeroPass.Model.Configuration;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;
using ZeroPass.Storage;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Service
{
    public class TokenService : ITokenService
    {
        public const string UserIdClaimType = "UserId";
        public const string PersonalDomainIdClaimType = "PersonalDomainId";
        
        readonly IUnitOfWorkFactory UnitOfWorkFactory;
        readonly IConfiguration Configuration;

        public TokenService(IUnitOfWorkFactory unitOfWorkFactory, IConfiguration configuration)
        {
            UnitOfWorkFactory = unitOfWorkFactory;
            Configuration = configuration;
        }

        public async Task<string> Authenticate(string email)
        {
            using var unitOfWork = await UnitOfWorkFactory.CreateRead();
            var user = await unitOfWork.Users.GetByEmail(email);
            if (user == null) return null;

            var domains = await unitOfWork.DomainUsers.GetDomainsByUserId(user.Id);
            var userDomainViews = domains as UserDomainView[] ?? domains.ToArray();
            var personalDomain = userDomainViews.FirstOrDefault(d => d.Domain.DomainType == DomainType.Personal);
            if (personalDomain == null) return null;
    
            return GenerateJwtToken(
                new TokenModel(user.Id, personalDomain.Domain.Id));
        }
        
        public Task<string> RefreshToken(TokenModel token) => Task.FromResult(GenerateJwtToken(token));

        string GenerateJwtToken(TokenModel model)
        {
            var claims = new[] {
                new Claim(UserIdClaimType, model.UserId.ToString(), ClaimValueTypes.Integer),
                new Claim(PersonalDomainIdClaimType, model.PersonalDomainId.ToString(), ClaimValueTypes.Integer),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue("JWT_SECURITY_KEY")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = creds,
                Expires = DateTime.UtcNow.AddSeconds(int.Parse(Configuration.GetValue("JWT_EXPIRES_SECONDS")))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
        public static TokenModel GetToken(ClaimsPrincipal user)
        {
            var identity = (ClaimsIdentity)user.Identity;
            return new TokenModel(
                int.Parse(GetClaimsValue(identity, UserIdClaimType)),
                int.Parse(GetClaimsValue(identity, PersonalDomainIdClaimType)));
        }
        
        static string GetClaimsValue(ClaimsIdentity identity, string name)
            => identity.Claims.FirstOrDefault(c => c.Type == name)?.Value;
    }
}