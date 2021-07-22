using System.Collections.Generic;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Models;

namespace AspCqrs.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);

        Task<IEnumerable<string>> GetUserRolesAsync(string userId);

        Task<(Result result, string userId, IEnumerable<string> roles)> CreateUserAsync(string userName,
            string password);

        Task<(Result result, string userId, IEnumerable<string> roles)> SignInAsync(string userName,
            string password);

        Task<bool> AuthorizeAsync(string userId, string policyName);

        Task<bool> IsInRoleAsync(string userId, string role);

        Task<Result> DeleteUserAsync(string userId);
    }
}