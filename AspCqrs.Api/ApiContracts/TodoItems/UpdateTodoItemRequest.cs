using AspCqrs.Application.Common.Mapping;
using AspCqrs.Application.TodoItems.Commands.UpdateTodoItem;

namespace AspCqrs.Api.ApiContracts.TodoItems
{
    public class UpdateTodoItemRequest : IMapTo<UpdateTodoItemCommand>
    {
        public string Title { get; set; }

        public string Note { get; set; }
        
        public int Priority { get; set; }
    }
}