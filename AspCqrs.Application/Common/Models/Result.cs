using System.Collections.Generic;

namespace AspCqrs.Application.Common.Models
{
    public class Result
    {
        public Result(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors;
        }

        public bool Succeeded { get; set; }
        
        public IEnumerable<string> Errors { get; set; }
        
        public static Result Success()
        {
            return new Result(true, new List<string>());
        }

        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, errors);
        }
    }
}