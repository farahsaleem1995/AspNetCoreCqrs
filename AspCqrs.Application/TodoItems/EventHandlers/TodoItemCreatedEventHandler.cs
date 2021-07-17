using System;
using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Models;
using AspCqrs.Domain.Events;
using MediatR;

namespace AspCqrs.Application.TodoItems.EventHandlers
{
    public class TodoItemCreatedEventHandler : INotificationHandler<DomainEventNotification<TodoItemCreatedEvent>>
    {
        public Task Handle(DomainEventNotification<TodoItemCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;
            
            Console.WriteLine($"Todo Item With ID {domainEvent.TodoItem.Id} Has Been Created.");
            
            return Task.CompletedTask;
        }
    }
}