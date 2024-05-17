using CryptLearn.Modules.AccessControl.Core;
using CryptLearn.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Bootstraper")]
namespace CryptLearn.Modules.AccessControl.Api
{
    internal class AccessControlModule : IModule
    {
        public string Name => "AccessControl";

        public void Add(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCore(configuration);
        }

        public void Use(IApplicationBuilder app, IConfiguration configuration)
        {

        }
    }
}
