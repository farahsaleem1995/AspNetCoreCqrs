using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Exceptions;
using AspCqrs.Application.Common.Interfaces;
using MediatR;

namespace AspCqrs.Application.Users.Commands.SignUpCommand
{
    public class SignUpCommand : IRequest<TokenDto>
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, TokenDto>
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtService _jwtService;

        public SignUpCommandHandler(IIdentityService identityService, IJwtService jwtService)
        {
            _identityService = identityService;
            _jwtService = jwtService;
        }

        public async Task<TokenDto> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var (creatResult, userId, roles) =
                await _identityService.CreateUserAsync(request.UserName, request.Password);

            if (!creatResult.Succeeded) throw new CommonException(creatResult.Errors.FirstOrDefault());

            var (accessToken, refreshToken) =
                await _jwtService.Generate(userId, request.UserName, roles, cancellationToken);
            
            return new TokenDto(accessToken, refreshToken);
        }
    }
}