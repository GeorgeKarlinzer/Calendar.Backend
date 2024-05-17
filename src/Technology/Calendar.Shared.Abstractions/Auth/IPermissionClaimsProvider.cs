namespace Calendar.Shared.Abstractions.Auth;
public interface IPermissionClaimsProvider
{
    IEnumerable<string> GetClaims();
}
