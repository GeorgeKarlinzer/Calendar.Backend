using Calendar.Identity.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Calendar.Identity.Core.Services;

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
