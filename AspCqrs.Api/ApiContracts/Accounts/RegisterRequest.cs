using AspCqrs.Application.Common.Mapping;
using AspCqrs.Application.Users.Commands;

namespace AspCqrs.Api.ApiContracts.Accounts
{
    public class RegisterRequest : IMapTo<SignUpCommand>
    {
        public string UserName { get; set; }
        
        public string Password { get; set; }
    }
}