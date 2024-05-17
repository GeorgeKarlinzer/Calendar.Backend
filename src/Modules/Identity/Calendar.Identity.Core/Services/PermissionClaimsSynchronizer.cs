using CryptLearn.Modules.AccessControl.Core.Entities;
using CryptLearn.Modules.AccessControl.Core.Interfaces;
using CryptLearn.Shared.Abstractions.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CryptLearn.Modules.AccessControl.Core.Services
{
    internal class PermissionClaimsSynchronizer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public PermissionClaimsSynchronizer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var claimsProviders = scope.ServiceProvider.GetServices<IPermissionClaimsProvider>();
            var repository = scope.ServiceProvider.GetRequiredService<IPermissionClaimsRepository>();
            var claims = claimsProviders.SelectMany(x => x.GetClaims());
            var dbClaims = await repository.GetAll().ToListAsync(cancellationToken);
            var toDelete = dbClaims.Where(x => !claims.Contains(x.Value));
            var toAdd = claims.Where(x => !dbClaims.Any(y => y.Value == x)).Select(x => new PermissionClaim() { Type = "permissions", Value = x });
            foreach (var item in toDelete)
            {
                repository.Remove(item);
            }

            foreach (var item in toAdd)
            {
                await repository.AddAsync(item, cancellationToken);
            }

            await repository.SaveAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
