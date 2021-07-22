using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Exceptions;
using AspCqrs.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspCqrs.Application.TodoItems.Commands.DeleteTodoItem
{
    public class DeleteTodoItemCommand : IRequest
    {
        public DeleteTodoItemCommand(int id)
        {
            Id = id;
        }
        
        public int Id { get; set; }
    }
    
    public class TodoItemDeleteCommandHandler : IRequestHandler<DeleteTodoItemCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public TodoItemDeleteCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
        {
            var todoItem = await _dbContext.TodoItems.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            if (todoItem == null) throw new NotFoundException("Todo item", request.Id);

            _dbContext.TodoItems.Remove(todoItem);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}