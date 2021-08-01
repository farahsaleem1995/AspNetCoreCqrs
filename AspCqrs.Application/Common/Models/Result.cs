using System.Collections.Generic;
using System.Linq;

namespace AspCqrs.Application.Common.Models
{
    public class Result
    {
        internal Result(bool succeeded, IDictionary<string, string[]> errors)
        {
            Succeeded = succeeded;
            Errors = errors;
        }

        public bool Succeeded { get; set; }

        public IDictionary<string, string[]> Errors { get; set; }

        public static Result Success()
        {
            return new Result(true, new Dictionary<string, string[]>());
        }

        public static Result Failure(IDictionary<string, string[]> errors)
        {
            return new Result(false, errors);
        }

        public static Result Failure(string errorKey, string errorMessage)
        {
            return new Result(false, new Dictionary<string, string[]>
            {
                {errorKey, new[] {errorMessage}}
            });
        }
    }
}