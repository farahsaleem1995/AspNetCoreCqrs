using System.Collections.Generic;

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

        public static Result Failure(string key, string[] errors)
        {
            return new Result(false, new Dictionary<string, string[]>
                {
                    {key, errors}
                }
            );
        }
        
        public static Result Failure(string key, string error)
        {
            return new Result(false, new Dictionary<string, string[]>
                {
                    {key, new[] {error}}
                }
            );
        }
    }
}