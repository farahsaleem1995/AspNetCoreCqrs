using System.Threading.Tasks;
using AspCqrs.Api.Filters;
using AspCqrs.Application.TodoItems.Commands.CreateTodoItem;
using AspCqrs.Application.TodoItems.Commands.DeleteTodoItem;
using AspCqrs.Application.TodoItems.Commands.UpdateTodoItem;
using AspCqrs.Application.TodoItems.Queries.GetAllTodoItems;
using AspCqrs.Application.TodoItems.Queries.GetTodoItemById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspCqrs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [ApiExceptionFilter]
    public class TodoItemsController : Controller
    {
        private readonly IMediator _mediator;
        public TodoItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllTodoItemsQuery getAllQuery)
        {
            var result = await _mediator.Send(getAllQuery);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTodoItemCommand createCommand)
        {
            var result = await _mediator.Send(createCommand);

            return Ok(result);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _mediator.Send(new GetTodoItemByIdQuery
            {
                Id = id
            });

            return Ok(result);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTodoItemCommand updateCommand)
        {
            updateCommand.SetId(id);
            
            var result = await _mediator.Send(updateCommand);

            return Ok(result);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _mediator.Send(new DeleteTodoItemCommand
            {
                Id = id
            });

            return NoContent();
        }
    }
}