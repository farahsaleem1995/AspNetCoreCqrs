using System;
using System.Linq;
using AspCqrs.Application.Common.Interfaces;

namespace AspCqrs.Application.Common.Extensions
{
    public static class QueryableFilterExtensions
    {
        public static IQueryable<TQuery> Filter<TQueryFilter, TQuery, TFilter>(this IQueryable<TQuery> queryable,
            TFilter filter)
            where TQueryFilter : IQueryFilter<TQuery, TFilter>
        {
            var queryFilter = Activator.CreateInstance<TQueryFilter>();

            return queryFilter.Apply(queryable, filter);
        }
        
        public static IQueryable<TQuery> Filter<TQuery>(this IQueryable<TQuery> queryable,
            Func<IQueryable<TQuery>, IQueryable<TQuery>> queryFunc)
        {
            return queryFunc(queryable);
        }
    }
}