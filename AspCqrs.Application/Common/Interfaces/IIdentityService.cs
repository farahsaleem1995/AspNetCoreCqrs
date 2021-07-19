using System.Threading.Tasks;
using AspCqrs.Application.Common.Models;

namespace AspCqrs.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);
        
        Task<(Result result, string userId)> CreateUserAsync(string username, string password);

        Task<bool> AuthorizeAsync(string userId, string policyName);
        
        Task<bool> IsInRoleAsync(string userId, string role);
        
        Task<Result> DeleteUserAsync(string userId);
    }
}