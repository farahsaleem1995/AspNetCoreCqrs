using AspCqrs.Application.Common;
using AspCqrs.Application.Common.Mapping;
using AspCqrs.Domain.Entities;
using AutoMapper;

namespace AspCqrs.Application.TodoItems
{
    public class TodoItemDto : AuditableDto, IMapFrom<TodoItem>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Note { get; set; }
        
        public int Priority { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TodoItem, TodoItemDto>()
                .ForMember(d => d.Priority, opt => opt.MapFrom(s => (int)s.Priority));
        }
    }
}