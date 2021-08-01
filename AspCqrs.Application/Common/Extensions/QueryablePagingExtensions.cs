using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace AspCqrs.Application.Common.Extensions
{
    public static class QueryablePagingExtensions
    {
        public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(this IQueryable<T> queryable, IPagingQuery pagingQuery, CancellationToken cancellationToken = default(CancellationToken))
        {
            var take = pagingQuery.PageSize == 0 ? 10 : pagingQuery.PageSize;
            var skip = pagingQuery.Page > 0 ? (pagingQuery.Page - 1) * take : 1;

            return new PaginatedList<T>
            {
                Total = await queryable.CountAsync(cancellationToken),
                Items = await queryable.Skip(skip).Take(take).ToListAsync(cancellationToken)
            };
        }
    }
}