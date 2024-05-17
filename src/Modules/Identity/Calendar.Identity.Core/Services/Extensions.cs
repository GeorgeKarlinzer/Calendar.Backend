using CryptLearn.Modules.AccessControl.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CryptLearn.Modules.AccessControl.Core.Services
{
    internal static class Extensions
    {
        public static IServiceCollection AddPasswordValidator(this IServiceCollection services)
            => services.AddScoped<IPasswordValidator, PasswordValidator>(x => new(y =>
            {
                y.RequireDigit = true;
                y.RequireLowercase = true;
                y.RequireUppercase = true;
                y.RequireNonAlphanumeric = true;
                y.RequiredLength = 10;
            }));

    }
}
