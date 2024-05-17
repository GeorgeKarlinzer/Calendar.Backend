using Microsoft.AspNetCore.Authorization;

namespace Calendar.Shared.Abstractions.Auth
{
    public class AuthorizeByClaimAttribute : AuthorizeAttribute
    {
        public AuthorizeByClaimAttribute(string permissionClaim) : base(permissionClaim) { }
    }
}
