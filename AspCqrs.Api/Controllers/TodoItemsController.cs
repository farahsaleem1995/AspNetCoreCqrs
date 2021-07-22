using System;
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
    public class TodoItemsController : ApiBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllTodoItemsQuery getAllQuery)
        {
            var result = await Mediator.Send(getAllQuery);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTodoItemCommand createCommand)
        {
            var result = await Mediator.Send(createCommand);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await Mediator.Send(new GetTodoItemByIdQuery(id));
            
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTodoItemCommand updateCommand)
        {
            updateCommand.Id = id;

            var result = await Mediator.Send(updateCommand);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await Mediator.Send(new DeleteTodoItemCommand(id));

            return Ok(result);
        }
    }
}