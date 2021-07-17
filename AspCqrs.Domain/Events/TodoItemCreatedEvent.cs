using AspCqrs.Domain.Common;
using AspCqrs.Domain.Entities;

namespace AspCqrs.Domain.Events
{
    public class TodoItemCreatedEvent : DomainEvent
    {
        public TodoItemCreatedEvent(TodoItem todoItem)
        {
            TodoItem = todoItem;
        }

        public TodoItem TodoItem { get; set; }
    }
}