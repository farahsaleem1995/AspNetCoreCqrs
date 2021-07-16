using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Interfaces;
using MediatR;

namespace AspCqrs.Application.Users.Commands
{
    public class RegisterCommand : IRequest<string>
    {
        public string Username { get; set; }
        
        public string Password { get; set; }
    }
    
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
    {
        private readonly IIdentityService _identityService;

        public RegisterCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        
        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.CreateUserAsync(request.Username, request.Password);

            return result.userId;
        }
    }
}