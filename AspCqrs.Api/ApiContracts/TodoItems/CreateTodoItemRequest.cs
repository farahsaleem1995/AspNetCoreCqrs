using AspCqrs.Application.Common.Mapping;
using AspCqrs.Application.TodoItems.Commands.CreateTodoItem;

namespace AspCqrs.Api.ApiContracts.TodoItems
{
    public class CreateTodoItemRequest : IMapTo<CreateTodoItemCommand>
    {
        public string Title { get; set; }

        public string Note { get; set; }
        
        public int Priority { get; set; }

        public string UserId { get; set; }
    }
}