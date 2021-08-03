using System.Threading.Tasks;
using AspCqrs.Application.Common.Enums;
using AspCqrs.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace AspCqrs.Api.Services
{
    public abstract class StreamServiceBase<THub, THubInterface, TData> : IStreamService<TData>
        where THub : Hub<THubInterface> 
        where THubInterface : class
    {
        private readonly IHubContext<THub, THubInterface> _hubContext;

        protected StreamServiceBase(IHubContext<THub, THubInterface> hubContext)
        {
            _hubContext = hubContext;
        }
        
        public async Task NotifyUser(string userId, StreamChange change, TData data)
        {
            var client = _hubContext.Clients.User(userId);

            await Notify(client, change, data);
        }

        public async Task NotifyGroup(string group, StreamChange change, TData data)
        {
            var client = _hubContext.Clients.Group(group);
            
            await Notify(client, change, data);
        }

        public async Task NotifyAll(StreamChange change, TData data)
        {
            var client = _hubContext.Clients.All;
            
            await Notify(client, change, data);
        }

        protected abstract Task Notify(THubInterface client, StreamChange change, TData data);
    }
}