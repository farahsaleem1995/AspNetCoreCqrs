using System;
using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.Common.Models;
using MediatR;

namespace AspCqrs.Application.Users.Commands
{
    public class SignInCommand : IRequest<JwtResult>
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
    
    public class SignInCommandHandler : IRequestHandler<SignInCommand, JwtResult>
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtService _jwtService;

        public SignInCommandHandler(IIdentityService identityService, IJwtService jwtService)
        {
            _identityService = identityService;
            _jwtService = jwtService;
        }

        public async Task<JwtResult> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.CheckUserNameAndPasswordAsync(request.Username, request.Password);

            if (!result.Succeeded) throw new UnauthorizedAccessException();

            var userId = await _identityService.GetUserIdAsync(request.Username);
            
            var jwtResult = await _jwtService.Generate(userId, cancellationToken);

            return jwtResult;
        }
    }
}