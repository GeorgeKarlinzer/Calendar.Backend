using Calendar.Shared.Abstractions.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Calendar.Shared.Infrastructure.Auth;

public class CalendarJwtBearerEvents(IJwtBearerEvents events) : JwtBearerEvents
{
    public override Task AuthenticationFailed(AuthenticationFailedContext context)
        => events.AuthenticationFailed(context);

    public override Task Challenge(JwtBearerChallengeContext context)
        => events.Challenge(context);

    public override Task Forbidden(ForbiddenContext context)
        => events.Forbidden(context);

    public override Task MessageReceived(MessageReceivedContext context)
        => events.MessageReceived(context);

    public override Task TokenValidated(TokenValidatedContext context)
        => events.TokenValidated(context);
}