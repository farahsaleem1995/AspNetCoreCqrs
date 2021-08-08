using System;
using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Enums;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.Common.Models;
using AspCqrs.Domain.Entities;
using AspCqrs.Domain.Events;
using AutoMapper;
using MediatR;

namespace AspCqrs.Application.TodoItems.EventHandlers
{
    public class TodoItemCreatedEventHandler : INotificationHandler<DomainEventNotification<TodoItemCreatedEvent>>
    {
        private readonly ICacheService _cacheService;
        private readonly IStreamService<TodoItemDto> _streamService;
        private readonly IMapper _mapper;

        public TodoItemCreatedEventHandler(ICacheService cacheService,
            IStreamService<TodoItemDto> streamService,
            IMapper mapper)
        {
            _cacheService = cacheService;
            _streamService = streamService;
            _mapper = mapper;
        }

        public async Task Handle(DomainEventNotification<TodoItemCreatedEvent> notification,
            CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            Console.WriteLine($"Todo Item With ID {domainEvent.TodoItem.Id} Has Been Created.");

            await _cacheService.RemoveAsync(TodoItemCacheKey.TodoItems.ToString());

            var data = _mapper.Map<TodoItem, TodoItemDto>(domainEvent.TodoItem);
            
            await _streamService.SendToAllAsync(StreamChange.Add, data);
        }
    }
}