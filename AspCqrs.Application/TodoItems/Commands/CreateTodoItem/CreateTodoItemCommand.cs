using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Exceptions;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.Common.Mapping;
using AspCqrs.Application.Common.Models;
using AspCqrs.Application.Common.Security;
using AspCqrs.Domain.Entities;
using AspCqrs.Domain.Events;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspCqrs.Application.TodoItems.Commands.CreateTodoItem
{
    [Authorize]
    public class CreateTodoItemCommand : IRequest<int>, IMapTo<TodoItem>
    {
        public string Title { get; set; }

        public string Note { get; set; }
        
        public int Priority { get; set; }

        public string UserId { get; set; }
    }
    
    public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, int>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateTodoItemCommandHandler(IApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.DomainUsers.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken);
            if (user == null) throw new NotFoundException("User", request.UserId);
            
            var todoItem = _mapper.Map<CreateTodoItemCommand, TodoItem>(request);
            
            todoItem.DomainEvents.Add(new TodoItemCreatedEvent(todoItem));

            _dbContext.TodoItems.Add(todoItem);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return todoItem.Id;
        }
    }
}