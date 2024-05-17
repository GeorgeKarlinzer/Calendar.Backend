using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Calendar.Shared.Abstractions.Auth
{
    public interface IJwtTokenFactory
    {
        JwtSecurityToken Create(string token);
        JwtSecurityToken Create(string issuer = null, string audience = null, IEnumerable<Claim> claims = null, DateTime? notBefore = null, DateTime? expires = null, SigningCredentials signingCredentials = null);
    }
}
