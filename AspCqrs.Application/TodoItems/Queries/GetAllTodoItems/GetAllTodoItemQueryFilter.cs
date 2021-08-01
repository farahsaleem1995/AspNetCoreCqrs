using System.Linq;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Domain.Entities;

namespace AspCqrs.Application.TodoItems.Queries.GetAllTodoItems
{
    public class GetAllTodoItemQueryFilter : IQueryFilter<TodoItem, GetAllTodoItemsQuery>
    {
        public IQueryable<TodoItem> Apply(IQueryable<TodoItem> queryable, GetAllTodoItemsQuery filter)
        {
            if (filter.Priority.HasValue)
                queryable = queryable.Where(item => item.Priority == filter.Priority.Value);

            return queryable;
        }
    }
}