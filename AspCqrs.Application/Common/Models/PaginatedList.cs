using System.Collections.Generic;
using AspCqrs.Application.Common.Mapping;
using AutoMapper;

namespace AspCqrs.Application.Common.Models
{
    public class PaginatedList<T>
    {
        public int Total { get; set; }
        
        public IEnumerable<T> Items { get; set; }
    }
}