using AspCqrs.Application.Common.Mapping;
using AspCqrs.Application.Common.Models;

namespace AspCqrs.Application.Users
{
    public class TokenDto
    {
        public TokenDto(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
        
        public string AccessToken { get; set; }
        
        public string RefreshToken { get; set; }
    }
}