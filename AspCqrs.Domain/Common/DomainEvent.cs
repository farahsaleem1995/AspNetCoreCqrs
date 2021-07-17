using System;
using System.Collections.Generic;

namespace AspCqrs.Domain.Common
{
    public interface IHasDomainEvent
    {
        public List<DomainEvent> DomainEvents { get; set; }
    }

    public class DomainEvent
    {
        public DomainEvent()
        {
            DateOccured = DateTimeOffset.UtcNow;
        }

        public bool IsPublished { get; set; }
        
        public DateTimeOffset DateOccured { get; protected set; }
    }
}