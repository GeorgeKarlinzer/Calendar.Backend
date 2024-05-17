using CryptLearn.Modules.AccessControl.Core.Commands;
using CryptLearn.Modules.AccessControl.Core.DTOs;
using CryptLearn.Modules.AccessControl.Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptLearn.Modules.AccessControl.Core.Controllers
{
    [Route("api/[controller]/[action]")]
    internal class AccessControlController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccessControlController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> TestAuth()
        {
            return await Task.FromResult(Ok());
        }

        [HttpPost]
        public async Task<ActionResult> SignUp([FromBody] SignUp command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> SignIn([FromBody] SignIn command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Logout(CancellationToken cancellationToken)
        {
            await _mediator.Send(new Logout(), cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePassword command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ProfileDto>> GetProfile(CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetProfile(), cancellationToken));
        }
    }
}
