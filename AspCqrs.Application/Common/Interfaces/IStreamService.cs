using System.Threading.Tasks;
using AspCqrs.Application.Common.Enums;

namespace AspCqrs.Application.Common.Interfaces
{
    public interface IStreamService<in TData>
    {
        Task NotifyUser(string userId, StreamChange change, TData data);

        Task NotifyGroup(string group, StreamChange change, TData data);
        
        Task NotifyAll(StreamChange change, TData data);
    }
}