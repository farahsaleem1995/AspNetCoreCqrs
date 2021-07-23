using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AspCqrs.Api.Controllers
{
    public abstract class CrudControllerBase<TGetAllQuery, TGetByIdQuery, TCreateCommand, TUpdateCommand,
        TDeleteCommand> : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] TGetAllQuery getAllQuery)
        {
            var result = await Mediator.Send(getAllQuery);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var query = Activator.CreateInstance(typeof(TGetByIdQuery), id);

            if (query == null) throw new ArgumentException("Null query");

            var result = await Mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TCreateCommand createCommand)
        {
            var result = await Mediator.Send(createCommand);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TUpdateCommand updateCommand)
        {
            var result = int.TryParse(typeof(TUpdateCommand).GetProperty("Id")?.GetValue(updateCommand)?.ToString(),
                out var bodyId);

            if (!result || bodyId != id)
                throw new FailedRequestException(new Dictionary<string, string[]>
                {
                    {
                        "IdMisMatch",
                        new[] {$"Route ID value ({id}) does not match body ID Property value ({bodyId})."}
                    }
                });
            
            await Mediator.Send(updateCommand);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var command = Activator.CreateInstance(typeof(TDeleteCommand), id);

            if (command == null) throw new ArgumentException("Null command");

            await Mediator.Send(command);

            return NoContent();
        }
    }
}