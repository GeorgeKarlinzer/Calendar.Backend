using Calendar.Shared.Abstractions.Auth;
using Calendar.Shared.Abstractions.Time;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Calendar.Shared.Infrastructure.Auth
{
    public sealed class AuthManager : IAuthManager
    {
        private readonly IClock _clock;
        private readonly IJwtTokenHandler _jwtTokenHandler;
        private readonly IJwtTokenFactory _jwtTokenFactory;
        private readonly SigningCredentials _signingCredentials;
        private readonly string _issuer;

        public AuthManager(AuthOptions options, IClock clock, IJwtTokenHandler jwtTokenHandler, IJwtTokenFactory jwtTokenFactory)
        {
            var issuerSigningKey = options.IssuerSigningKey;
            if (issuerSigningKey is null)
            {
                throw new InvalidOperationException("Issuer signing key not set.");
            }
            _clock = clock;
            _jwtTokenHandler = jwtTokenHandler;
            _jwtTokenFactory = jwtTokenFactory;
            _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.IssuerSigningKey)), SecurityAlgorithms.HmacSha256);
            _issuer = options.Issuer;
        }

        public string CreateToken(Guid userId, DateTime expireDate, IEnumerable<Claim> claims = null)
        {
            var now = _clock.CurrentDate();
            var jwtClaims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeMilliseconds().ToString())
            };

            jwtClaims.AddRange(claims ?? Enumerable.Empty<Claim>());

            var jwt = _jwtTokenFactory.Create(
                _issuer,
                claims: jwtClaims,
                notBefore: now,
                expires: expireDate,
                signingCredentials: _signingCredentials
            );

            var token = _jwtTokenHandler.WriteToken(jwt);

            return token;
        }
    }
}