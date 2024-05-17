using Microsoft.AspNetCore.Authorization;

namespace Calendar.Shared.Abstractions.Auth;

public class AuthorizeByClaimAttribute(string permissionClaim) : AuthorizeAttribute(permissionClaim);
