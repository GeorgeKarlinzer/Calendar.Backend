using CryptLearn.Modules.AccessControl.Core.Entities;
using CryptLearn.Modules.AccessControl.Core.Exceptions;
using CryptLearn.Modules.AccessControl.Core.Interfaces;
using CryptLearn.Modules.AccessControl.Core.Services;
using CryptLearn.Shared.Abstractions.Auth;
using CryptLearn.Shared.Abstractions.Cqrs;
using CryptLearn.Shared.Abstractions.Time;
using CryptLearn.Shared.Infrastructure.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace CryptLearn.Modules.AccessControl.Core.Commands;
internal class CommandsHandler : ICommandHandler<SignIn>, ICommandHandler<SignUp>, ICommandHandler<ChangePassword>, ICommandHandler<Logout>, ICommandHandler<RotateToken, string>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IEnumerable<IPermissionClaimsProvider> _claimsProviders;
    private readonly IJwtTokenFactory _tokenFactory;
    private readonly IJwtTokenHandler _tokenHandler;
    private readonly IAuthManager _authManager;
    private readonly IUsersRepository _usersRepository;
    private readonly IClock _clock;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly JwtOptions _jwtOptions;

    public CommandsHandler(IHttpContextAccessor contextAccessor, IConfiguration configuration, IEnumerable<IPermissionClaimsProvider> claimsProviders, IJwtTokenFactory tokenFactory, IJwtTokenHandler tokenHandler, IAuthManager authManager, IUsersRepository usersRepository, IClock clock, IPasswordHasher<User> passwordHasher, JwtOptions jwtOptions)
    {
        _contextAccessor = contextAccessor;
        _configuration = configuration;
        _claimsProviders = claimsProviders;
        _tokenFactory = tokenFactory;
        _tokenHandler = tokenHandler;
        _authManager = authManager;
        _usersRepository = usersRepository;
        _clock = clock;
        _passwordHasher = passwordHasher;
        _jwtOptions = jwtOptions;
    }

    public async Task Handle(SignIn request, CancellationToken cancellationToken)
    {
        var cookies = _contextAccessor.HttpContext.Response.Cookies;

        var user = await _usersRepository
            .GetAll()
            .Include(x => x.RefreshTokens)
            .Include(x => x.Claims)
            .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

        var tokensToDelete = user.RefreshTokens.Where(x => x.ValidTo < _clock.CurrentDate()).ToList();
        foreach (var tok in tokensToDelete)
        {
            user.RefreshTokens.Remove(tok);
        }

        var accessTokenExpireDate = _clock.CurrentDate().Add(_jwtOptions.AccessTokenExpiry);
        var refreshTokenExpireDate = _clock.CurrentDate().Add(_jwtOptions.RefreshTokenExpiry);

        var accessToken = _authManager.CreateToken(user.Id, accessTokenExpireDate, user.Claims.Select(x => new Claim(x.Type, x.Value)).Append(new("userName", user.UserName)));
        var refreshToken = _authManager.CreateToken(user.Id, refreshTokenExpireDate);

        user.RefreshTokens.Add(new UserRefreshToken(refreshTokenExpireDate, refreshToken));
        await _usersRepository.SaveAsync(cancellationToken);

        cookies.Append(_jwtOptions.AccessTokenCookieName, accessToken, GetCookieOptions(accessTokenExpireDate));
        cookies.Append(_jwtOptions.RefreshTokenCookieName, refreshToken, GetCookieOptions(refreshTokenExpireDate));
    }

    public async Task Handle(SignUp request, CancellationToken cancellationToken)
    {
        if (await _usersRepository.GetAll().AnyAsync(x => x.NormalizedEmail == request.Email.ToLowerInvariant()
                                                       || x.NormalizedUserName == request.UserName.ToLowerInvariant()
                                                       , cancellationToken))
        {
            return;
        }

        var password = _passwordHasher.HashPassword(default, request.Password);
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName,
            NormalizedUserName = request.UserName.ToLowerInvariant(),
            Email = request.Email,
            NormalizedEmail = request.Email.ToLowerInvariant(),
            PasswordHash = password,
            SecurityStamp = Guid.NewGuid().ToString(),
            Claims = new()
        };

        var userClaims = _claimsProviders.SelectMany(x => x.GetClaims()).Select(x => new UserClaim() { Type = "permissions", Value = x });

        var configPath = "Modules:AccessControl:DefaultClaims";

        if (_configuration.GetValue<string>(configPath) is not "*")
        {
            var claims = _configuration.GetSection(configPath).Get<IEnumerable<string>>() ?? Enumerable.Empty<string>();
            userClaims = userClaims.Where(x => claims.Contains(x.Value));
        }

        user.Claims.AddRange(userClaims);

        await _usersRepository.AddAsync(user, cancellationToken);
        await _usersRepository.SaveAsync(cancellationToken);
    }

    public async Task Handle(ChangePassword request, CancellationToken cancellationToken)
    {
        var cookies = _contextAccessor.HttpContext.Response.Cookies;
        var userId = Guid.Parse(_contextAccessor.HttpContext.User.Identity.Name);
        var user = await _usersRepository.GetAll().Include(x => x.RefreshTokens).FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
        user.RefreshTokens.Clear();
        user.SecurityStamp = Guid.NewGuid().ToString();
        var oldUser = await _usersRepository.GetAll().FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (oldUser.SecurityStamp != user.SecurityStamp)
        {
            throw new ConcurrencyException();
        }
        await _usersRepository.SaveAsync(cancellationToken);
        cookies.Delete(_jwtOptions.RefreshTokenCookieName);
        cookies.Delete(_jwtOptions.AccessTokenCookieName);
    }

    public Task Handle(Logout request, CancellationToken cancellationToken)
    {
        var cookies = _contextAccessor.HttpContext.Response.Cookies;
        cookies.Delete(_jwtOptions.AccessTokenCookieName);
        cookies.Delete(_jwtOptions.RefreshTokenCookieName);
        return Task.CompletedTask;
    }

    public async Task<string> Handle(RotateToken request, CancellationToken cancellationToken)
    {
        var requestCookies = _contextAccessor.HttpContext.Request.Cookies;
        var responseCookies = _contextAccessor.HttpContext.Response.Cookies;

        requestCookies.TryGetValue(_jwtOptions.AccessTokenCookieName, out var token);

        if (token is not null)
        {
            var result = await _tokenHandler.ValidateTokenAsync(token, request.Options.TokenValidationParameters);
            if (result.IsValid)
            {
                return token;
            }
            responseCookies.Delete(_jwtOptions.AccessTokenCookieName);
        }

        requestCookies.TryGetValue(_jwtOptions.RefreshTokenCookieName, out var refreshToken);
        if (refreshToken is null)
        {
            return null;
        }
        responseCookies.Delete(_jwtOptions.RefreshTokenCookieName);
        var jwtToken = _tokenFactory.Create(refreshToken);
        var userId = Guid.Parse(jwtToken.Subject);
        var user = await _usersRepository.GetAll().Include(x => x.RefreshTokens).Include(x => x.Claims).FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        var result2 = await _tokenHandler.ValidateTokenAsync(refreshToken, request.Options.TokenValidationParameters);
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
            await _usersRepository.SaveAsync(cancellationToken);
            return null;
        }

        userRefreshToken.IsActive = false;
        await _usersRepository.SaveAsync(cancellationToken);

        var accessTokenExpireDate = _clock.CurrentDate().Add(_jwtOptions.AccessTokenExpiry);
        var refreshTokenExpireDate = _clock.CurrentDate().Add(_jwtOptions.RefreshTokenExpiry);

        var newAccessToken = _authManager.CreateToken(userId, accessTokenExpireDate, user.Claims.Select(x => new Claim(x.Type, x.Value)).Append(new("userName", user.UserName)));
        var newRefreshToken = _authManager.CreateToken(userId, refreshTokenExpireDate);

        responseCookies.Append(_jwtOptions.AccessTokenCookieName, newAccessToken, GetCookieOptions(accessTokenExpireDate));
        responseCookies.Append(_jwtOptions.RefreshTokenCookieName, newRefreshToken, GetCookieOptions(refreshTokenExpireDate));

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
