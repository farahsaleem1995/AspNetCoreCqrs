using System;
using AutoMapper;

namespace AspCqrs.Api.ApiContracts
{
    public abstract class ApiRequest<TCommand>
    {
        public TCommand GetCommandOrQuery(IMapper mapper, Action<TCommand> afterMap = null)
        {
            var command = mapper.Map<TCommand>(this);

            afterMap?.Invoke(command);

            return command;
        }
    }
}