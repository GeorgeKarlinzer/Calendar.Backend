using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Calendar.Shared.Abstractions.Auth;

public interface IJwtTokenHandler
{
    Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters validationParameters);
    string WriteToken(JwtSecurityToken jwt);
}
