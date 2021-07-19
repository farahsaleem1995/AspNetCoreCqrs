using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspCqrs.Application.TodoItems.Queries.GetTodoItemById
{
    public class GetTodoItemByIdQuery: IRequest<TodoItemDto>
    {
        public int Id { get; set; }
    }
    
    public class GetTodoItemByIdQueryHandler : IRequestHandler<GetTodoItemByIdQuery, TodoItemDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetTodoItemByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<TodoItemDto> Handle(GetTodoItemByIdQuery request, CancellationToken cancellationToken)
        {
            var todoItems = await _dbContext.TodoItems
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            return _mapper.Map<TodoItem, TodoItemDto>(todoItems);
        }
    }
}