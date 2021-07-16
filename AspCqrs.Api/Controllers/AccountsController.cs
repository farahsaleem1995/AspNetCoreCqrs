using System.Threading.Tasks;
using AspCqrs.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspCqrs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
    }
}