using System;

namespace AspCqrs.Application.Common.Models
{
    public abstract class AbstractUpdateCommand<TId>
        where TId : IConvertible, IComparable<TId>, IEquatable<TId>
    {
        public TId Id { get; private set; }
        
        public void SetId(TId id)
        {
            Id = id;
        }
    }
}