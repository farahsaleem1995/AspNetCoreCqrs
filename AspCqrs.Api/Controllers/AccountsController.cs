using System.Threading.Tasks;
using AspCqrs.Api.Filters;
using AspCqrs.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspCqrs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExceptionFilter]
    public class AccountsController : Controller
    {
        private readonly IMediator _mediator;
        
        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterCommand registerCommand)
        {
            var result = await _mediator.Send(registerCommand);

            return Ok(result);
        }
        
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInCommand signInCommand)
        {
            var result = await _mediator.Send(signInCommand);

            return Ok(result);
        }
    }
}