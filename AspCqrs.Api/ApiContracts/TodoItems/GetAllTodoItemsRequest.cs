using AspCqrs.Application.Common.Mapping;
using AspCqrs.Application.TodoItems.Queries.GetAllTodoItems;

namespace AspCqrs.Api.ApiContracts.TodoItems
{
    public class GetAllTodoItemsRequest : ApiRequest<GetAllTodoItemsQuery>, IMapTo<GetAllTodoItemsQuery>
    {
        public int Page { get; set; }

        public byte PageSize { get; set; }
    }
}