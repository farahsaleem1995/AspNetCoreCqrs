using System.Linq;

namespace AspCqrs.Application.Common.Interfaces
{
    public interface IQueryFilter<TQuery, in TFilter>
    {
        IQueryable<TQuery> Apply(IQueryable<TQuery> queryable, TFilter filter);
    }
}