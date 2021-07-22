using System.Threading.Tasks;
using AspCqrs.Application.Users.Commands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspCqrs.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : ApiBaseController
    {
        public AccountsController(IMediator mediator, IMapper mapper) 
            : base(mediator, mapper)
        {
        }
        
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] SignUpCommand signUpCommand)
        {
            var result = await Mediator.Send(signUpCommand);

            return new ObjectResult(result);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInCommand signInCommand)
        {
            var result = await Mediator.Send(signInCommand);

            return new ObjectResult(result);
        }
    }
}