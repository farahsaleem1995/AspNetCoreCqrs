using System.Threading.Tasks;
using AspCqrs.Application.TodoItems.Commands.CreateTodoItem;
using AspCqrs.Application.TodoItems.Commands.DeleteTodoItem;
using AspCqrs.Application.TodoItems.Commands.UpdateTodoItem;
using AspCqrs.Application.TodoItems.Queries.GetAllTodoItems;
using AspCqrs.Application.TodoItems.Queries.GetTodoItemById;
using Microsoft.AspNetCore.Mvc;

namespace AspCqrs.Api.Controllers
{
    [Route("api/[controller]")]
    public class TodoItemsController : CrudControllerBase<GetAllTodoItemsQuery, 
        GetTodoItemByIdQuery,
        CreateTodoItemCommand, 
        UpdateTodoItemCommand, 
        DeleteTodoItemCommand>
    {
    }
}