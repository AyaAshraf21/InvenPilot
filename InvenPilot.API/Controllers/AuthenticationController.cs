using InvenPilot.Application.Features.Authentication.Commands;
using InvenPilot.Application.Features.Authentication.DTO;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvenPilot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthenticationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            await mediator.Send(new RegisterUserCommand(registerDTO));
            return Ok();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var token = await mediator.Send(new  LoginUserCommand(loginDTO));
            return Ok(token);
        }
    }
}
