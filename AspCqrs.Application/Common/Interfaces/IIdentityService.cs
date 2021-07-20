using System.Collections.Generic;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Models;

namespace AspCqrs.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);
        
        Task<string> GetUserIdAsync(string userName);

        Task<Result> CheckUserNameAndPasswordAsync(string userName, string password);

        Task<IEnumerable<string>> GetUserRolesAsync(string userId);

        Task<(Result result, string userId)> CreateUserAsync(string userName, string password);

        Task<bool> AuthorizeAsync(string userId, string policyName);
        
        Task<bool> IsInRoleAsync(string userId, string role);
        
        Task<Result> DeleteUserAsync(string userId);
    }
}