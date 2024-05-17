using System.Security.Claims;

namespace Calendar.Shared.Abstractions.Auth;

public interface IAuthManager
{
    string CreateToken(Guid userId, DateTime expireDate, IEnumerable<Claim> claims = null);
}