using System.Threading.Tasks;

namespace AspCqrs.Application.Common.Interfaces
{
    public interface IDbInitializer
    {
        Task Initialize();
    }
}