using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.Common.Mapping;
using AspCqrs.Domain.Entities;
using AutoMapper;
using MediatR;

namespace AspCqrs.Application.TodoItems.Commands
{
    public class CreateTodoItemCommand : IRequest<TodoItemDto>, IMapTo<TodoItem>
    {
        public string Title { get; set; }

        public string Note { get; set; }
        
        public int Priority { get; set; }
    }
    
    public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, TodoItemDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateTodoItemCommandHandler(IApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<TodoItemDto> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
        {
            var todoItem = _mapper.Map<CreateTodoItemCommand, TodoItem>(request);

            _dbContext.TodoItems.Add(todoItem);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TodoItem, TodoItemDto>(todoItem);
        }
    }
}