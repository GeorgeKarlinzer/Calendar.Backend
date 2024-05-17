using Calendar.Shared.Abstractions.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Calendar.Shared.Infrastructure.Auth
{
    public class PolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly AuthorizationPolicy _defaultPolicy;
        private readonly IEnumerable<string> _authSchemes;
        private readonly IServiceProvider _serviceProvider;

        public PolicyProvider(IServiceProvider serviceProvider)
        {
            _authSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };
            _defaultPolicy = new AuthorizationPolicy(new[] { new DenyAnonymousAuthorizationRequirement() }, _authSchemes);
            _serviceProvider = serviceProvider;
        }

        public async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            await Task.CompletedTask;
            using var scope = _serviceProvider.CreateScope();
            var policies = scope.ServiceProvider.GetServices<IPolicy>();
            var requirements = policies.FirstOrDefault(x => x.GetType().FullName == policyName)
                ?.Requirements.ToArray() ?? new[] { new ClaimsAuthorizationRequirement("permissions", new[] { policyName }) };

            return new AuthorizationPolicyBuilder()
                .AddRequirements(requirements)
                .Build();
        }
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(_defaultPolicy);
        }

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return Task.FromResult<AuthorizationPolicy>(null);
        }
    }
}