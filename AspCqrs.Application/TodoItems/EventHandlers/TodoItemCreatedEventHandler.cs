using System;
using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.Common.Models;
using AspCqrs.Domain.Events;
using MediatR;

namespace AspCqrs.Application.TodoItems.EventHandlers
{
    public class TodoItemCreatedEventHandler : INotificationHandler<DomainEventNotification<TodoItemCreatedEvent>>
    {
        private readonly ICacheService _cacheService;

        public TodoItemCreatedEventHandler(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task Handle(DomainEventNotification<TodoItemCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;
            
            Console.WriteLine($"Todo Item With ID {domainEvent.TodoItem.Id} Has Been Created.");
            
            await _cacheService.RemoveAsync(TodoItemCacheKey.TodoItems.ToString());
        }
    }
}