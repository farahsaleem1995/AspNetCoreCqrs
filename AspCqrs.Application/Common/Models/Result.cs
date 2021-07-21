using System.Collections.Generic;

namespace AspCqrs.Application.Common.Models
{
    public class Result<TData>
    {
        public Result(bool succeeded, TData data, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Data = data;
            Errors = errors;
        }

        public bool Succeeded { get; set; }

        public TData Data { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }

    public class Result : Result<object>
    {
        public Result(bool succeeded, object data, IEnumerable<string> errors) 
            : base(succeeded, data, errors)
        {
        }
        
        public static Result Success()
        {
            return new Result(true, default, default);
        }

        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, default, errors);
        }
        
        public static Result<TData> Success<TData>(TData data)
        {
            return new Result<TData>(true, data, default);
        }

        public static Result<TData> Failure<TData>(IEnumerable<string> errors)
        {
            return new Result<TData>(false, default, errors);
        }
    }
}