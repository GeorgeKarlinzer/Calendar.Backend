using CryptLearn.Modules.AccessControl.Core.Commands;
using CryptLearn.Shared.Abstractions.Time;
using CryptLearn.Shared.Infrastructure.Auth;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace CryptLearn.Modules.AccessControl.Core.Services
{
    internal class JwtBearerEvents : IJwtBearerEvents
    {
        private readonly IMediator _mediator;

        public JwtBearerEvents(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task MessageReceived(MessageReceivedContext context)
        {
            var authorizeAttribute = context.HttpContext.GetEndpoint()?.Metadata?.GetMetadata<AuthorizeAttribute>();
            if (authorizeAttribute is not null)
            {
                context.Token = await _mediator.Send(new RotateToken(context.Options));
            }
        }
    }
}
