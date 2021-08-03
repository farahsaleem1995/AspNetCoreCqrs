using System.Threading.Tasks;
using AspCqrs.Application.TodoItems;

namespace AspCqrs.Api.Hubs.Interfaces
{
    public interface ITodoItemHub
    {
        Task AddTodoItem(TodoItemDto todoItemDto);
        
        Task UpdateTodoItem(TodoItemDto todoItemDto);
        
        Task RemoveTodoItem(TodoItemDto todoItemDto);
    }
}