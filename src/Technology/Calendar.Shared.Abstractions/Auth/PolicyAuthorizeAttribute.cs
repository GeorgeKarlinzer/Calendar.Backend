using Microsoft.AspNetCore.Authorization;

namespace Calendar.Shared.Abstractions.Auth
{
    public class AuthorizeByPolicyAttribute<TPolicy> : AuthorizeAttribute
        where TPolicy : IPolicy
    {
        public AuthorizeByPolicyAttribute()
        {
            Policy = typeof(TPolicy).FullName;
        }
    }
}
