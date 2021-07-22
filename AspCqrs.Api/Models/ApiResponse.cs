using System.Collections.Generic;

namespace AspCqrs.Api.Models
{
    public class ApiResponse<TData>
    {
        public bool Succeeded { get; set; }

        public IDictionary<string, string[]> Errors { get; set; }

        public TData Data { get; set; }
    }
}