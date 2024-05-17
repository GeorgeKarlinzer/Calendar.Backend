using Calendar.Identity.Core.DAL.DbContexts;
using Calendar.Identity.Core.Entities;
using Calendar.Identity.Core.Services;
using Calendar.Shared.Abstractions.Auth;
using Calendar.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Calendar.Identity.Core;

public class IdentityModule : IModule
{
    public string Name => "Identity";

    public void Add(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("Modules:Identity:ConnectionString");

        services.AddDbContext<IdentityDbContext>(x => x.UseSqlServer(connectionString))
                .AddHostedService<PermissionClaimsSynchronizer>()
                .AddScoped<IJwtBearerEvents, JwtBearerEvents>()
                .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>()
                .AddPasswordValidator();
    }

    public Task Use(IApplicationBuilder app, IConfiguration configuration, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
