using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Calendar.Shared.Abstractions.Modules;

public interface IModule
{
    string Name { get; }
    void Add(IServiceCollection services, IConfiguration configuration);
    Task Use(IApplicationBuilder app, IConfiguration configuration, CancellationToken cancellationToken = default);
}
