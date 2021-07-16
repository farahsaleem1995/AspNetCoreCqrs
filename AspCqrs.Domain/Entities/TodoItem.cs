using AspCqrs.Domain.Common;
using AspCqrs.Domain.Enums;

namespace AspCqrs.Domain.Entities
{
    public class TodoItem : AuditableEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Note { get; set; }

        public PriorityLevel Priority { get; set; }
    }
}