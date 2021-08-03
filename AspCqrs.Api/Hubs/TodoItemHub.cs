using AspCqrs.Api.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace AspCqrs.Api.Hubs
{
    public class TodoItemHub : Hub<ITodoItemHub>
    {
    }
}