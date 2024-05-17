using Calendar.Identity.Core.Commands;
using Calendar.Shared.Abstractions.Auth;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Calendar.Identity.Core.Services;

internal class JwtBearerEvents(IMediator mediator) : IJwtBearerEvents
{
    public async Task MessageReceived(MessageReceivedContext context)
    {
        var authorizeAttribute = context.HttpContext.GetEndpoint()?.Metadata?.GetMetadata<AuthorizeAttribute>();
        if (authorizeAttribute is not null)
        {
            context.Token = await mediator.Send(new RotateToken(context.Options));
        }
    }
}
