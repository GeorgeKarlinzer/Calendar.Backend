using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Calendar.Shared.Abstractions.Auth;

public interface IJwtBearerEvents
{
    public Task AuthenticationFailed(AuthenticationFailedContext context)
        => Task.CompletedTask;
    public Task Forbidden(ForbiddenContext context)
        => Task.CompletedTask;
    public Task MessageReceived(MessageReceivedContext context)
        => Task.CompletedTask;
    public Task TokenValidated(TokenValidatedContext context)
        => Task.CompletedTask;
    public Task Challenge(JwtBearerChallengeContext context)
        => Task.CompletedTask;
}