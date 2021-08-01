using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AspCqrs.Application.Common.Interfaces;

namespace AspCqrs.Application.Common.Extensions
{
    public static class QueryableSortExtensions
    {
        public static IOrderedQueryable<T> Sort<T>(this IQueryable<T> queryable, IDictionary<string, Expression<Func<T, object>>> sortDictionary, ISortQuery sortQuery)
        {
            if (string.IsNullOrEmpty(sortQuery.SortBy)) return sortQuery.IsSortAscending
                ? queryable.OrderBy(arg => arg)
                : queryable.OrderByDescending(arg => arg);
            
            return sortQuery.IsSortAscending
                ? queryable.OrderBy(sortDictionary[sortQuery.SortBy.ToUpper()])
                : queryable.OrderByDescending(sortDictionary[sortQuery.SortBy.ToUpper()]);
        }
    }
}