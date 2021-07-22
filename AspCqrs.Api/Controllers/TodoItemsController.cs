using System.Collections.Generic;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Models;
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
    [Route("api/[controller]")]
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
        public async Task<IActionResult> Create([FromBody] CreateTodoItemCommand createCommand)
        {
            var result = await Mediator.Send(createCommand);

            return new ObjectResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await Mediator.Send(new GetTodoItemByIdQuery(id));

            return new ObjectResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTodoItemCommand updateCommand)
        {
            if (updateCommand.Id != id)
                return new ObjectResult(Result.BadRequest(new Dictionary<string, string[]>
                {
                    {"InvalidID", new[] {"ID does not match"}}
                }));

            var result = await Mediator.Send(updateCommand);

            return new ObjectResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await Mediator.Send(new DeleteTodoItemCommand(id));

            return new ObjectResult(result);
        }
    }
}