using System.Threading.Tasks;
using AspCqrs.Api.ApiContracts.TodoItems;
using AspCqrs.Api.Attributes;
using AspCqrs.Api.Filters;
using AspCqrs.Application.TodoItems.Commands.CreateTodoItem;
using AspCqrs.Application.TodoItems.Commands.DeleteTodoItem;
using AspCqrs.Application.TodoItems.Commands.UpdateTodoItem;
using AspCqrs.Application.TodoItems.Queries.GetAllTodoItems;
using AspCqrs.Application.TodoItems.Queries.GetTodoItemById;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspCqrs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExceptionFilter]
    [ApiResponse]
    public class TodoItemsController : ApiBaseController
    {
        public TodoItemsController(IMediator mediator, IMapper mapper)
            : base(mediator, mapper)
        {
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllTodoItemsQuery getAllQuery)
        {
            var result = await Mediator.Send(getAllQuery);

            return new ObjectResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTodoItemRequest createRequest)
        {
            var result = await Mediator.Send(Mapper.Map<CreateTodoItemRequest, CreateTodoItemCommand>(createRequest));

            return new ObjectResult(result);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await Mediator.Send(new GetTodoItemByIdQuery
            {
                Id = id
            });

            return new ObjectResult(result);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTodoItemRequest updateRequest)
        {
            var command = Mapper.Map<UpdateTodoItemRequest, UpdateTodoItemCommand>(updateRequest);
            command.SetId(id);
            
            var result = await Mediator.Send(command);

            return new ObjectResult(result);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result =await Mediator.Send(new DeleteTodoItemCommand
            {
                Id = id
            });

            return new ObjectResult(result);
        }
    }
}