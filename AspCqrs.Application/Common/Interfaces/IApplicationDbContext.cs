using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspCqrs.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<TodoItem> TodoItems { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}