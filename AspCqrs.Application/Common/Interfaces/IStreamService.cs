using System.Threading.Tasks;
using AspCqrs.Application.Common.Enums;

namespace AspCqrs.Application.Common.Interfaces
{
    public interface IStreamService<in TData>
    {
        Task SendToUserAsync(string userId, StreamChange change, TData data);

        Task SendToGroupAsync(string group, StreamChange change, TData data);
        
        Task SendToAllAsync(StreamChange change, TData data);
    }
}