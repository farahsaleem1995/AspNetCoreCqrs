using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.Common.Models;
using MediatR;

namespace AspCqrs.Application.Users.Commands
{
    public class RegisterCommand : IRequest<JwtResult>
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, JwtResult>
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtService _jwtService;

        public RegisterCommandHandler(IIdentityService identityService, IJwtService jwtService)
        {
            _identityService = identityService;
            _jwtService = jwtService;
        }

        public async Task<JwtResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var (_, userId, roles) = await _identityService.CreateUserAsync(request.Username, request.Password);

            var jwtResult = await _jwtService.Generate(userId, request.Username, roles, cancellationToken);

            return jwtResult;
        }
    }
}