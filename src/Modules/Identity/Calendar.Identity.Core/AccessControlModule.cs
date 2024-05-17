using CryptLearn.Modules.AccessControl.Core.DAL.DbContexts;
using CryptLearn.Modules.AccessControl.Core.DAL.Repositories;
using CryptLearn.Modules.AccessControl.Core.Entities;
using CryptLearn.Modules.AccessControl.Core.Interfaces;
using CryptLearn.Modules.AccessControl.Core.Services;
using CryptLearn.Shared.Abstractions.Auth;
using CryptLearn.Shared.Abstractions.Modules;
using CryptLearn.Shared.Infrastructure.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CryptLearn.Modules.AccessControl.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace CryptLearn.Modules.AccessControl.Core;

public class AccessControlModule : IModule
{
    public string Name => "AccessControl";

    public void Add(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("Modules:AccessControl:ConnectionString");

        services.AddDbContext<AccessControlDbContext>(x => x.UseSqlServer(connectionString))
                .AddScoped<IUsersRepository, UsersRepository>()
                .AddScoped<IPermissionClaimsRepository, PermissionClaimsRepository>()
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
