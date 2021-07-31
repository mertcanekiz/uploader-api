using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Uploader.Application.Common.Interfaces;
using Uploader.Application.Identity.Commands;
using Uploader.Application.Identity.Commands.Login;
using Uploader.Application.Identity.Commands.Register;

namespace Uploader.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}