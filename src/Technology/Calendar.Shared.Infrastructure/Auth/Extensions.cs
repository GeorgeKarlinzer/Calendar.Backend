using Calendar.Shared.Abstractions.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Calendar.Shared.Infrastructure.Auth;

public static class Extensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var authOptions = configuration.GetOptions<AuthOptions>("TokenValidationParameters");
        var jwtOptions = configuration.GetOptions<JwtOptions>("JwtConfiguration");
        services.AddSingleton(jwtOptions);
        services.AddSingleton(authOptions);
        services.AddScoped<CalendarJwtBearerEvents>();
        services.AddScoped<IAuthManager, AuthManager>();

        if (authOptions.AuthenticationDisabled)
        {
            services.AddSingleton<IPolicyEvaluator, DisabledAuthenticationPolicyEvaluator>();
        }

        var tokenValidationParameters = new TokenValidationParameters
        {
            RequireAudience = authOptions.RequireAudience,
            ValidIssuer = authOptions.ValidIssuer,
            ValidIssuers = authOptions.ValidIssuers,
            ValidateActor = authOptions.ValidateActor,
            ValidAudience = authOptions.ValidAudience,
            ValidAudiences = authOptions.ValidAudiences,
            ValidateAudience = authOptions.ValidateAudience,
            ValidateIssuer = authOptions.ValidateIssuer,
            ValidateLifetime = authOptions.ValidateLifetime,
            ValidateTokenReplay = authOptions.ValidateTokenReplay,
            ValidateIssuerSigningKey = authOptions.ValidateIssuerSigningKey,
            SaveSigninToken = authOptions.SaveSigninToken,
            RequireExpirationTime = authOptions.RequireExpirationTime,
            RequireSignedTokens = authOptions.RequireSignedTokens,
            ClockSkew = TimeSpan.Zero
        };

        if (string.IsNullOrWhiteSpace(authOptions.IssuerSigningKey))
        {
            throw new ArgumentException("Missing issuer signing key.", nameof(authOptions.IssuerSigningKey));
        }

        if (!string.IsNullOrWhiteSpace(authOptions.AuthenticationType))
        {
            tokenValidationParameters.AuthenticationType = authOptions.AuthenticationType;
        }

        var rawKey = Encoding.UTF8.GetBytes(authOptions.IssuerSigningKey);
        tokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(rawKey);

        if (!string.IsNullOrWhiteSpace(authOptions.NameClaimType))
        {
            tokenValidationParameters.NameClaimType = authOptions.NameClaimType;
        }

        if (!string.IsNullOrWhiteSpace(authOptions.RoleClaimType))
        {
            tokenValidationParameters.RoleClaimType = authOptions.RoleClaimType;
        }

        services
            .AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.Authority = authOptions.Authority;
                o.Audience = authOptions.Audience;
                o.MetadataAddress = authOptions.MetadataAddress;
                o.SaveToken = authOptions.SaveToken;
                o.RefreshOnIssuerKeyNotFound = authOptions.RefreshOnIssuerKeyNotFound;
                o.RequireHttpsMetadata = authOptions.RequireHttpsMetadata;
                o.IncludeErrorDetails = authOptions.IncludeErrorDetails;
                o.TokenValidationParameters = tokenValidationParameters;
                if (!string.IsNullOrWhiteSpace(authOptions.Challenge))
                {
                    o.Challenge = authOptions.Challenge;
                }
                o.EventsType = typeof(CalendarJwtBearerEvents);
            });

        services.AddSingleton(authOptions);
        services.AddSingleton(tokenValidationParameters);
        services.AddFromAssemblies<IPolicy>((c, s, i) => c.AddSingleton(s, i));
        services.AddSingleton<IAuthorizationPolicyProvider, PolicyProvider>();
        services.AddAuthorization();
        services.AddScoped<IJwtTokenHandler, JwtTokenHandlerWrapper>();
        services.AddScoped<IJwtTokenFactory, JwtTokenFactory>();

        return services;
    }
}