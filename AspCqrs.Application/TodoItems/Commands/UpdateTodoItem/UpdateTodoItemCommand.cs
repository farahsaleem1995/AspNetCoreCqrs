using System;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Exceptions;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.Common.Mapping;
using AspCqrs.Application.Common.Models;
using AspCqrs.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspCqrs.Application.TodoItems.Commands.UpdateTodoItem
{
    public class UpdateTodoItemCommand : AbstractUpdateCommand<int>, IRequest<Result>, IMapTo<TodoItem>
    {
        public string Title { get; set; }

        public string Note { get; set; }
        
        public int Priority { get; set; }
    }
    
    public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand, Result>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateTodoItemCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
        {
            var todoItem = await _dbContext.TodoItems
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken: cancellationToken);
            if (todoItem == null) throw new NotFoundException("Todo item", request.Id);

            _mapper.Map(request, todoItem);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}