using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.Common.Models;
using AspCqrs.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspCqrs.Application.TodoItems.Queries.GetTodoItemById
{
    public class GetTodoItemByIdQuery: IRequest<Result<TodoItemDto>>
    {
        public int Id { get; set; }
    }
    
    public class GetTodoItemByIdQueryHandler : IRequestHandler<GetTodoItemByIdQuery, Result<TodoItemDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetTodoItemByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Result<TodoItemDto>> Handle(GetTodoItemByIdQuery request, CancellationToken cancellationToken)
        {
            var todoItems = await _dbContext.TodoItems
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            return Result.Success(_mapper.Map<TodoItem, TodoItemDto>(todoItems));
        }
    }
}