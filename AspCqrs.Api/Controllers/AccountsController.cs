using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Api.ApiContracts;
using AspCqrs.Api.Filters;
using AspCqrs.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AspCqrs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExceptionFilter]
    public class AccountsController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtService _jwtService;

        public AccountsController(IIdentityService identityService, IJwtService jwtService)
        {
            _identityService = identityService;
            _jwtService = jwtService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest,
            CancellationToken cancellationToken)
        {
            var creatResult = await _identityService.CreateUserAsync(registerRequest.UserName, registerRequest.UserName);

            if (!creatResult.Succeeded) return Unauthorized(creatResult);

            var jwtResult = await _jwtService.Generate(creatResult.Data.userId, registerRequest.UserName, creatResult.Data.roles,
                cancellationToken);
            
            if (!creatResult.Succeeded) return Unauthorized(creatResult);

            return Ok(jwtResult);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest signInRequest, CancellationToken cancellationToken)
        {
            var creatResult = await _identityService.SignInAsync(signInRequest.UserName, signInRequest.Password);
            
            if (!creatResult.Succeeded) return Unauthorized(creatResult);

            var jwtResult = await _jwtService.Generate(creatResult.Data.userId, signInRequest.UserName, creatResult.Data.roles,
                cancellationToken);
            
            if (!jwtResult.Succeeded) return Unauthorized(jwtResult);

            return Ok(jwtResult);
        }
    }
}