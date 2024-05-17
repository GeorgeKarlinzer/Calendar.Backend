using Calendar.Identity.Core.Commands;
using Calendar.Identity.Core.DTOs;
using Calendar.Identity.Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.Identity.Core.Controllers;

[Route("api/[controller]/[action]")]
internal class IdentityController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> TestAuth()
    {
        return await Task.FromResult(Ok());
    }

    [HttpPost]
    public async Task<ActionResult> SignUp([FromBody] SignUp command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> SignIn([FromBody] SignIn command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Logout(CancellationToken cancellationToken)
    {
        await mediator.Send(new Logout(), cancellationToken);
        return Ok();
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePassword command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<ProfileDto>> GetProfile(CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetProfile(), cancellationToken));
    }
}
