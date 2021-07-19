using System;
using AspCqrs.Domain.Common;

namespace AspCqrs.Domain.Entities
{
    public class DomainUser
    {
        public string Id { get; set; }
        
        public string UserName { get; set; }

        public DateTime Created { get; set; }
    }
}