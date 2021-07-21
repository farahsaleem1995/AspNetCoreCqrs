using System.Threading.Tasks;
using AspCqrs.Api.ApiContracts.Accounts;
using AspCqrs.Api.Attributes;
using AspCqrs.Api.Filters;
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
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var result = await Mediator.Send(Mapper.Map<RegisterRequest, SignUpCommand>(registerRequest));

            return new ObjectResult(result);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest signInRequest)
        {
            var result = await Mediator.Send(Mapper.Map<SignInRequest, SignInCommand>(signInRequest));

            return new ObjectResult(result);
        }
    }
}