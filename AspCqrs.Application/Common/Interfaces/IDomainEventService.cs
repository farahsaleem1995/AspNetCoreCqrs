using System.Threading.Tasks;
using AspCqrs.Domain.Common;

namespace AspCqrs.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}