using CryptLearn.Modules.AccessControl.Core.DTOs;
using CryptLearn.Modules.AccessControl.Core.Services;
using CryptLearn.Shared.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace CryptLearn.Modules.AccessControl.Api.Controllers
{
    [ApiController]
    [Route("access-control")]
    internal class AccessControlController : ControllerBase
    {
        private readonly IAccessControlService _accessControlService;

        public AccessControlController(IAccessControlService service)
        {
            _accessControlService = service;
        }

        [HttpGet("test-auth")]
        [Authorize]
        public async Task<ActionResult> TestAuth()
        {
            return await Task.FromResult(Ok());
        }

        [HttpPost("sign-up")]
        public async Task<ActionResult> SignUpAsync(SignUpDTO dto)
        {
            await _accessControlService.SignUpAsync(dto);
            return Ok();
        }

        [HttpPost("sign-in")]
        public async Task<ActionResult> SignInAsync(SignInDTO dto)
        {
            await _accessControlService.SignInAsync(dto, Response.Cookies);
            return Ok();
        }

        [HttpPost("sign-out")]
        [Authorize]
        public async Task<ActionResult> SignOutAsync()
        {
            await _accessControlService.SignOutAsync(Response.Cookies);
            return Ok();
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult> ChangePasswordAsync(ChangePasswordDTO dto)
        {
            await _accessControlService.ChangePassword(Guid.Parse(User.Identity.Name), dto, HttpContext.Response.Cookies);
            return Ok();
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetProfileInformationAsync()
        {
            var dto = await _accessControlService.GetProfileInformationAsync(Guid.Parse(User.Identity.Name));
            return Ok(dto);
        }
    }
}
