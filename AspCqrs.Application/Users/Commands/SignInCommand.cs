using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.Common.Models;
using AutoMapper;
using MediatR;

namespace AspCqrs.Application.Users.Commands
{
    public class SignInCommand : IRequest<Result<TokenDto>>
    {
        public string UserName { get; set; }
        
        public string Password { get; set; }
    }
    
    public class SignInCommandHandler : IRequestHandler<SignInCommand, Result<TokenDto>>
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public SignInCommandHandler(IIdentityService identityService, 
            IJwtService jwtService,
            IMapper mapper)
        {
            _identityService = identityService;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<Result<TokenDto>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var signInResult = await _identityService.SignInAsync(request.UserName, request.Password);
            
            if (!signInResult.Succeeded) return Result<TokenDto>.Unauthorized(signInResult.Errors);

            var jwtResult = await _jwtService.Generate(signInResult.Data.userId, request.UserName, signInResult.Data.roles,
                cancellationToken);
            
            if (!jwtResult.Succeeded) return Result<TokenDto>.Unauthorized(jwtResult.Errors);

            return Result<TokenDto>.Success(_mapper.Map<JwtResult, TokenDto>(jwtResult.Data));
        }
    }
}