using System;
using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Interfaces;
using MediatR;

namespace AspCqrs.Application.Users.Commands.SignInCommand
{
    public class SignInCommand : IRequest<TokenDto>
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class SignInCommandHandler : IRequestHandler<SignInCommand, TokenDto>
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtService _jwtService;

        public SignInCommandHandler(IIdentityService identityService, IJwtService jwtService)
        {
            _identityService = identityService;
            _jwtService = jwtService;
        }

        public async Task<TokenDto> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var (signInResult, userId, roles) = await _identityService.SignInAsync(request.UserName, request.Password);

            if (!signInResult.Succeeded) throw new UnauthorizedAccessException("Username and password does not match");

            var (accessToken, refreshToken) =
                await _jwtService.Generate(userId, request.UserName, roles, cancellationToken);

            return new TokenDto(accessToken, refreshToken);
        }
    }
}