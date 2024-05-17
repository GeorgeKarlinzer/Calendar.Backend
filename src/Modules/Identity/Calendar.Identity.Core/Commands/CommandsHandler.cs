using Calendar.Identity.Core.Entities;
using Calendar.Identity.Core.Exceptions;
using Calendar.Shared.Abstractions.Auth;
using Calendar.Shared.Abstractions.Cqrs;
using Calendar.Shared.Abstractions.Services;
using Calendar.Shared.Abstractions.Time;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Calendar.Identity.Core.Commands;
internal class CommandsHandler(IHttpContextAccessor contextAccessor, IConfiguration configuration, IEnumerable<IPermissionClaimsProvider> claimsProviders, IJwtTokenFactory tokenFactory, IJwtTokenHandler tokenHandler, IAuthManager authManager, IRepository<User> usersRepository, IClock clock, IPasswordHasher<User> passwordHasher, JwtOptions jwtOptions)
    :
    ICommandHandler<SignIn>,
    ICommandHandler<SignUp>,
    ICommandHandler<ChangePassword>,
    ICommandHandler<Logout>,
    ICommandHandler<RotateToken, string>
{
    public async Task Handle(SignIn request, CancellationToken cancellationToken)
    {
        var cookies = contextAccessor.HttpContext.Response.Cookies;

        var user = await usersRepository
            .GetAll()
            .Include(x => x.RefreshTokens)
            .Include(x => x.Claims)
            .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

        var tokensToDelete = user.RefreshTokens.Where(x => x.ValidTo < clock.CurrentDate()).ToList();
        foreach (var tok in tokensToDelete)
        {
            user.RefreshTokens.Remove(tok);
        }

        var accessTokenExpireDate = clock.CurrentDate().Add(jwtOptions.AccessTokenExpiry);
        var refreshTokenExpireDate = clock.CurrentDate().Add(jwtOptions.RefreshTokenExpiry);

        var accessToken = authManager.CreateToken(user.Id, accessTokenExpireDate, user.Claims.Select(x => new Claim(x.Type, x.Value)).Append(new("userName", user.UserName)));
        var refreshToken = authManager.CreateToken(user.Id, refreshTokenExpireDate);

        user.RefreshTokens.Add(new UserRefreshToken(refreshTokenExpireDate, refreshToken));
        await usersRepository.SaveAsync(cancellationToken);

        cookies.Append(jwtOptions.AccessTokenCookieName, accessToken, GetCookieOptions(accessTokenExpireDate));
        cookies.Append(jwtOptions.RefreshTokenCookieName, refreshToken, GetCookieOptions(refreshTokenExpireDate));
    }

    public async Task Handle(SignUp request, CancellationToken cancellationToken)
    {
        if (await usersRepository.GetAll().AnyAsync(x => x.NormalizedEmail.Equals(request.Email, StringComparison.InvariantCultureIgnoreCase)
                                                       || x.NormalizedUserName.Equals(request.UserName, StringComparison.InvariantCultureIgnoreCase)
                                                       , cancellationToken))
        {
            return;
        }

        var password = passwordHasher.HashPassword(default, request.Password);
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName,
            NormalizedUserName = request.UserName.ToLowerInvariant(),
            Email = request.Email,
            NormalizedEmail = request.Email.ToLowerInvariant(),
            PasswordHash = password,
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        var userClaims = claimsProviders.SelectMany(x => x.GetClaims()).Select(x => new UserClaim() { Type = "permissions", Value = x });

        var configPath = "Modules:Identity:DefaultClaims";

        if (configuration.GetValue<string>(configPath) is not "*")
        {
            var claims = configuration.GetSection(configPath).Get<IEnumerable<string>>() ?? Enumerable.Empty<string>();
            userClaims = userClaims.Where(x => claims.Contains(x.Value));
        }

        user.Claims.AddRange(userClaims);

        await usersRepository.AddAsync(user, cancellationToken);
        await usersRepository.SaveAsync(cancellationToken);
    }

    public async Task Handle(ChangePassword request, CancellationToken cancellationToken)
    {
        var cookies = contextAccessor.HttpContext.Response.Cookies;
        var userId = Guid.Parse(contextAccessor.HttpContext.User.Identity.Name);
        var user = await usersRepository.GetAll().Include(x => x.RefreshTokens).FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        user.PasswordHash = passwordHasher.HashPassword(user, request.NewPassword);
        user.RefreshTokens.Clear();
        user.SecurityStamp = Guid.NewGuid().ToString();
        var oldUser = await usersRepository.GetAll().FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (oldUser.SecurityStamp != user.SecurityStamp)
        {
            throw new ConcurrencyException();
        }
        await usersRepository.SaveAsync(cancellationToken);
        cookies.Delete(jwtOptions.RefreshTokenCookieName);
        cookies.Delete(jwtOptions.AccessTokenCookieName);
    }

    public Task Handle(Logout request, CancellationToken cancellationToken)
    {
        var cookies = contextAccessor.HttpContext.Response.Cookies;
        cookies.Delete(jwtOptions.AccessTokenCookieName);
        cookies.Delete(jwtOptions.RefreshTokenCookieName);
        return Task.CompletedTask;
    }

    public async Task<string> Handle(RotateToken request, CancellationToken cancellationToken)
    {
        var requestCookies = contextAccessor.HttpContext.Request.Cookies;
        var responseCookies = contextAccessor.HttpContext.Response.Cookies;

        requestCookies.TryGetValue(jwtOptions.AccessTokenCookieName, out var token);

        if (token is not null)
        {
            var result = await tokenHandler.ValidateTokenAsync(token, request.Options.TokenValidationParameters);
            if (result.IsValid)
            {
                return token;
            }
            responseCookies.Delete(jwtOptions.AccessTokenCookieName);
        }

        requestCookies.TryGetValue(jwtOptions.RefreshTokenCookieName, out var refreshToken);
        if (refreshToken is null)
        {
            return null;
        }
        responseCookies.Delete(jwtOptions.RefreshTokenCookieName);
        var jwtToken = tokenFactory.Create(refreshToken);
        var userId = Guid.Parse(jwtToken.Subject);
        var user = await usersRepository.GetAll().Include(x => x.RefreshTokens).Include(x => x.Claims).FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        var result2 = await tokenHandler.ValidateTokenAsync(refreshToken, request.Options.TokenValidationParameters);
        if (!result2.IsValid)
        {
            return null;
        }

        var userRefreshToken = user.RefreshTokens.FirstOrDefault(x => x.Value == refreshToken);
        if (userRefreshToken is null)
        {
            return null;
        }

        // if used refresh token is inactive, then revoke all user's refresh tokens
        if (!userRefreshToken.IsActive)
        {
            user.RefreshTokens.Clear();
            await usersRepository.SaveAsync(cancellationToken);
            return null;
        }

        userRefreshToken.IsActive = false;
        await usersRepository.SaveAsync(cancellationToken);

        var accessTokenExpireDate = clock.CurrentDate().Add(jwtOptions.AccessTokenExpiry);
        var refreshTokenExpireDate = clock.CurrentDate().Add(jwtOptions.RefreshTokenExpiry);

        var newAccessToken = authManager.CreateToken(userId, accessTokenExpireDate, user.Claims.Select(x => new Claim(x.Type, x.Value)).Append(new("userName", user.UserName)));
        var newRefreshToken = authManager.CreateToken(userId, refreshTokenExpireDate);

        responseCookies.Append(jwtOptions.AccessTokenCookieName, newAccessToken, GetCookieOptions(accessTokenExpireDate));
        responseCookies.Append(jwtOptions.RefreshTokenCookieName, newRefreshToken, GetCookieOptions(refreshTokenExpireDate));

        return newAccessToken;
    }

    private static CookieOptions GetCookieOptions(DateTime expireDate)
        => new()
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            IsEssential = true,
            Secure = true,
            Expires = expireDate.AddSeconds(-5)
        };
}
