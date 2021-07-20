using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.Common.Models;
using AspCqrs.Application.Options;
using AspCqrs.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AspCqrs.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IIdentityService _identityService;
        private readonly IApplicationDbContext _dbContext;
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> options,
            IIdentityService identityService,
            IApplicationDbContext dbContext)
        {
            _identityService = identityService;
            _dbContext = dbContext;
            _jwtSettings = options.Value;
        }

        public async Task<JwtResult> Generate(string userId, CancellationToken cancellationToken)
        {
            var userName = await _identityService.GetUserNameAsync(userId);

            var roles = await _identityService.GetUserRolesAsync(userId);
            
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.Add(_jwtSettings.LifeTime);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = signingCredentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = userId,
                ExpireAt = DateTime.UtcNow.AddMonths(6)
            };
            _dbContext.RefreshTokens.Add(refreshToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new JwtResult
            {
                AccessToken = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.JwtId
            };
        }
    }
}