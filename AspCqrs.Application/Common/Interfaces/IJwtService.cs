using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Models;

namespace AspCqrs.Application.Common.Interfaces
{
    public interface IJwtService
    {
        Task<JwtResult> Generate(string userId, CancellationToken cancellationToken);
    }
}