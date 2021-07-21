using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.Common.Models;
using AutoMapper;
using MediatR;

namespace AspCqrs.Application.Users.Commands
{
    public class SignUpCommand : IRequest<Result<TokenDto>>
    {
        public string UserName { get; set; }
        
        public string Password { get; set; }
    }
    
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, Result<TokenDto>>
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public SignUpCommandHandler(IIdentityService identityService, 
            IJwtService jwtService,
            IMapper mapper)
        {
            _identityService = identityService;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<Result<TokenDto>> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var creatResult = await _identityService.CreateUserAsync(request.UserName, request.UserName);

            if (!creatResult.Succeeded) return Result<TokenDto>.BadRequest(creatResult.Errors);

            var jwtResult = await _jwtService.Generate(creatResult.Data.userId, request.UserName, creatResult.Data.roles,
                cancellationToken);
            
            if (!creatResult.Succeeded) return Result<TokenDto>.BadRequest(creatResult.Errors);

            return Result<TokenDto>.Success(_mapper.Map<JwtResult, TokenDto>(jwtResult.Data));
        }
    }
}