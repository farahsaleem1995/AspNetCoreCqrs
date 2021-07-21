using AspCqrs.Application.Common.Mapping;
using AspCqrs.Application.Common.Models;

namespace AspCqrs.Application.Users
{
    public class TokenDto : IMapFrom<JwtResult>
    {
        public string AccessToken { get; set; }
        
        public string RefreshToken { get; set; }
    }
}