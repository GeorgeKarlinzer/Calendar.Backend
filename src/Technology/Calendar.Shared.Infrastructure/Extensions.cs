using AutoMapper;
using Calendar.Shared.Abstractions.Auth;
using Calendar.Shared.Abstractions.Modules;
using Calendar.Shared.Abstractions.Time;
using Calendar.Shared.Infrastructure.Auth;
using Calendar.Shared.Infrastructure.Exceptions;
using Calendar.Shared.Infrastructure.Services;
using Calendar.Shared.Infrastructure.Time;
using Calendar.Shared.Infrastructure.Validations;
using Calendar.Shared.Infrastructure.Api;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Calendar.Api")]
namespace Calendar.Shared.Infrastructure
{
    internal static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IList<IModule> modules)
        {
            services.AddSingleton<IClock, UtcClock>();
            services.AddControllers().ConfigureApplicationPartManager(m => m.FeatureProviders.Add(new InternalControllerFeatureProvider()));
            services.AddAuth(configuration);
            services.AddErrorHandling();
            services.AddHostedService<AppInitializer>();
            services.AddMediatR(x => x.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            services.AddValidation();
            services.AddSwaggerGen();
            services.AddHttpContextAccessor();
            services.AddCors(x =>
            {
                x.AddPolicy("cors", x =>
                {
                    x.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins(configuration.GetValue<string>("AllowedOriginsCors"));
                });
            });
            services.AddFromAssemblies<Profile>((c, s, i) => c.AddAutoMapper(i));
            services.AddFromAssemblies<IPermissionClaimsProvider>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseCors("cors");
            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseErrorHandling();
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }

        public static T GetOptions<T>(this IConfiguration configuration, string key) where T : new()
        {
            var options = new T();
            configuration.GetSection(key).Bind(options);
            return options;
        }

        internal static IServiceCollection AddFromAssemblies<TService>(this IServiceCollection services)
            => services.AddFromAssemblies<TService>((x, y, z) => x.AddScoped(y, z));

        internal static IServiceCollection AddFromAssemblies<TService>(this IServiceCollection services, Action<IServiceCollection, Type, Type> addServiceAction)
        {
            var type = typeof(TService);
            var implementations = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);

            foreach (var i in implementations)
            {
                addServiceAction(services, type, i);
            }

            return services;
        }
    }
}
