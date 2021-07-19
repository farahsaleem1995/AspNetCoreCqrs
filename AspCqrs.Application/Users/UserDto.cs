using AspCqrs.Application.Common;
using AspCqrs.Application.Common.Mapping;
using AspCqrs.Domain.Entities;

namespace AspCqrs.Application.Users
{
    public class UserDto : IMapFrom<User>
    {
        public string Id { get; set; }
        
        public string UserName { get; set; }
    }
}