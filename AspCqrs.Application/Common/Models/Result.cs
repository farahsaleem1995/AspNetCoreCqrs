using System;
using System.Collections.Generic;
using AspCqrs.Application.Common.Enums;

namespace AspCqrs.Application.Common.Models
{
    public class Result<TData>
    {
        public Result(bool succeeded, ResultStatus status, TData data, IDictionary<string, string[]> errors)
        {
            Succeeded = succeeded;
            Status = status.ToString();
            Data = data;
            Errors = errors;
        }

        public bool Succeeded { get; set; }

        public string Status { get; set; }

        public TData Data { get; set; }

        public IDictionary<string, string[]> Errors { get; set; }
        
        public static Result Success()
        {
            return new Result(true, ResultStatus.Success, default);
        }
        
        public static Result<TData> Success(TData data)
        {
            return new Result<TData>(true, ResultStatus.Success, data, default);
        }
        
        public static Result<TData> BadRequest(IDictionary<string, string[]> errors)
        {
            return new Result<TData>(false, ResultStatus.BadRequest, default, errors);
        }
        
        public static Result<TData> Forbidden()
        {
            return new Result<TData>(false, ResultStatus.Forbidden, default, null);
        }
        
        public static Result<TData> NotFound(string name, object key)
        {
            var errors = new Dictionary<string, string[]>
            {
                {"Source not found", new[] {$"Entity \"{name}\" ({key}) was not found."}}
            };

            return new Result<TData>(false, ResultStatus.NotFound, default, errors);
        }

        public static Result<TData> NotFound(string message)
        {
            var errors = new Dictionary<string, string[]>
            {
                {"Source not found", new[] {message}}
            };
            
            return new Result<TData>(false, ResultStatus.NotFound, default, errors);
        }
        
        public static Result<TData> Unauthorized()
        {
            var errors = new Dictionary<string, string[]>
            {
                {"Unauthorized", new[] {new UnauthorizedAccessException().Message, }}
            };
            
            return new Result<TData>(false, ResultStatus.Unauthorized, default, errors);
        }
        
        public static Result<TData> Unauthorized(IDictionary<string, string[]> errors)
        {
            return new Result<TData>(false, ResultStatus.Unauthorized, default, errors);
        }

        public Result ToEmptyResult()
        {
            return new Result(Succeeded, Enum.Parse<ResultStatus>(Status), Errors);
        }
    }

    public class Result : Result<object>
    {
        public Result(bool succeeded, ResultStatus status, IDictionary<string, string[]> errors)
            : base(succeeded, status, null, errors)
        {
        }
    }
}