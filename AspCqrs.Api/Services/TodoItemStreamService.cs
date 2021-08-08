using System;
using System.Threading.Tasks;
using AspCqrs.Api.Hubs;
using AspCqrs.Api.Hubs.Interfaces;
using AspCqrs.Application.Common.Enums;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.TodoItems;
using Microsoft.AspNetCore.SignalR;

namespace AspCqrs.Api.Services
{
    public class TodoItemStreamService : IStreamService<TodoItemDto>
    {
        private readonly IHubContext<TodoItemHub, ITodoItemHub> _hubContext;

        public TodoItemStreamService(IHubContext<TodoItemHub, ITodoItemHub> hubContext)
        {
            _hubContext = hubContext;
        }
        
        public async Task SendToUserAsync(string userId, StreamChange change, TodoItemDto data)
        {
            var client = _hubContext.Clients.User(userId);

            await Notify(client, change, data);
        }

        public async Task SendToGroupAsync(string group, StreamChange change, TodoItemDto data)
        {
            var client = _hubContext.Clients.Group(group);
            
            await Notify(client, change, data);
        }

        public async Task SendToAllAsync(StreamChange change, TodoItemDto data)
        {
            var client = _hubContext.Clients.All;
            
            await Notify(client, change, data);
        }

        private static async Task Notify(ITodoItemHub client, StreamChange change, TodoItemDto data)
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