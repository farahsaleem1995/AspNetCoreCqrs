using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Models;

namespace AspCqrs.Application.Common.Interfaces
{
    public interface IJwtService
    {
        Task<(string accessToken, string refreshToken)> Generate(string userId, 
            string userName, 
            IEnumerable<string> roles,
            CancellationToken cancellationToken);
    }
}