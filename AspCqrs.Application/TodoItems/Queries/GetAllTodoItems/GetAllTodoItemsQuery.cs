using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspCqrs.Application.TodoItems.Queries.GetAllTodoItems
{
    public class GetAllTodoItemsQuery : IRequest<IEnumerable<TodoItemDto>>
    {
        public int Page { get; set; }

        public byte PageSize { get; set; }
    }

    public class GetAllTodoItemsQueryHandler : IRequestHandler<GetAllTodoItemsQuery, IEnumerable<TodoItemDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllTodoItemsQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TodoItemDto>> Handle(GetAllTodoItemsQuery request,
            CancellationToken cancellationToken)
        {
            var todoItems = await _dbContext.TodoItems
                .Include(t => t.User)
                .OrderBy(t => t.Created)
                .Skip(request.Page < 1 ? 1 : (request.Page - 1) * (request.PageSize < 1 ? 10 : request.PageSize))
                .Take(request.PageSize < 1 ? 10 : request.PageSize)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<TodoItem>, IEnumerable<TodoItemDto>>(todoItems);
        }
    }
}