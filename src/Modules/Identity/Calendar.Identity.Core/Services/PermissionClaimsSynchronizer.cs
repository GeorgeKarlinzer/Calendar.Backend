using Calendar.Identity.Core.Entities;
using Calendar.Shared.Abstractions.Auth;
using Calendar.Shared.Abstractions.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Calendar.Identity.Core.Services;

internal class PermissionClaimsSynchronizer(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var claimsProviders = scope.ServiceProvider.GetServices<IPermissionClaimsProvider>();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<PermissionClaim>>();
        var claims = claimsProviders.SelectMany(x => x.GetClaims());
        var dbClaims = await repository.GetAll().ToListAsync(cancellationToken);
        var toDelete = dbClaims.Where(x => !claims.Contains(x.Value));
        var toAdd = claims.Where(x => !dbClaims.Any(y => y.Value == x)).Select(x => new PermissionClaim() { Type = "permissions", Value = x });
        foreach (var item in toDelete)
        {
            await repository.RemoveAsync(item, cancellationToken);
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
