using System;
using System.Threading.Tasks;
using AspCqrs.Api.Hubs;
using AspCqrs.Api.Hubs.Interfaces;
using AspCqrs.Application.Common.Enums;
using AspCqrs.Application.TodoItems;
using Microsoft.AspNetCore.SignalR;

namespace AspCqrs.Api.Services
{
    public class TodoItemStreamService : StreamServiceBase<TodoItemHub, ITodoItemHub, TodoItemDto>
    {
        public TodoItemStreamService(IHubContext<TodoItemHub, ITodoItemHub> hubContext)
            : base(hubContext)
        {
        }

        protected override async Task Notify(ITodoItemHub client, StreamChange change, TodoItemDto data)
        {
            switch (change)
            {
                case StreamChange.Add:
                    await client.AddTodoItem(data);
                    break;
                case StreamChange.Remove:
                    await client.RemoveTodoItem(data);
                    break;
                case StreamChange.Update:
                    await client.UpdateTodoItem(data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(change), change, null);
            }
        }
    }
}