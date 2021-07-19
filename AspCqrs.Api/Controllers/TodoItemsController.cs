using System.Threading.Tasks;
using AspCqrs.Api.Filters;
using AspCqrs.Application.TodoItems.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspCqrs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExceptionFilter]
    public class TodoItemsController : Controller
    {
        private readonly IMediator _mediator;
        public TodoItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTodoItemCommand createCommand)
        {
            var result = await _mediator.Send(createCommand);

            return Ok(result);
        }
    }
}