using AspCqrs.Domain.Common;

namespace AspCqrs.Domain.Entities
{
    public class DomainUser : AuditableEntity
    {
        public string Id { get; set; }
        
        public string UserName { get; set; }
    }
}