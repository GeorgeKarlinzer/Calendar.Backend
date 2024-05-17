using Calendar.Shared.Abstractions.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Calendar.Shared.Infrastructure.Auth;

internal class JwtTokenFactory : IJwtTokenFactory
{
    public JwtSecurityToken Create(string token)
    {
        var jwtToken = new JwtSecurityToken(token);
        return jwtToken;
    }

    public JwtSecurityToken Create(string issuer = null, string audience = null, IEnumerable<Claim> claims = null, DateTime? notBefore = null, DateTime? expires = null, SigningCredentials signingCredentials = null)
    {
        var jwtToken = new JwtSecurityToken(issuer, audience, claims, notBefore, expires, signingCredentials);
        return jwtToken;
    }
}
