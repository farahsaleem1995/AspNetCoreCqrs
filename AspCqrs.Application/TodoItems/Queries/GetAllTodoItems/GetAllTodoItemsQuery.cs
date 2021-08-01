using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Attributes;
using AspCqrs.Application.Common.Cache;
using AspCqrs.Application.Common.Extensions;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.Common.Models;
using AspCqrs.Domain.Entities;
using AspCqrs.Domain.Enums;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspCqrs.Application.TodoItems.Queries.GetAllTodoItems
{
    [Cached(TodoItemCacheKey.TodoItems)]
    public class GetAllTodoItemsQuery : IRequest<PaginatedList<TodoItemDto>>, IPagingQuery, ISortQuery
    {
        public PriorityLevel? Priority { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public string SortBy { get; set; }

        public bool IsSortAscending { get; set; }
    }

    public class GetAllTodoItemsQueryHandler : IRequestHandler<GetAllTodoItemsQuery, PaginatedList<TodoItemDto>>
    {
        private readonly IDictionary<string, Expression<Func<TodoItem, object>>> _sortDictionary =
            new Dictionary<string, Expression<Func<TodoItem, object>>>
            {
                {"created".ToUpper(), item => item.Created},
                {"title".ToUpper(), item => item.Title},
            };

        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllTodoItemsQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TodoItemDto>> Handle(GetAllTodoItemsQuery request,
            CancellationToken cancellationToken)
        {
            var todoItems = await _dbContext.TodoItems
                .Include(t => t.User)
                .Filter<GetAllTodoItemQueryFilter, TodoItem, GetAllTodoItemsQuery>(request)
                .Sort(_sortDictionary, request)
                .ToPaginatedListAsync(request, cancellationToken);

            return _mapper.Map<PaginatedList<TodoItem>, PaginatedList<TodoItemDto>>(todoItems);
        }
    }
}