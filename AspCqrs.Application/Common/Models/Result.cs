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
    }

    public class Result : Result<object>
    {
        public Result(bool succeeded, ResultStatus status, object data, IDictionary<string, string[]> errors)
            : base(succeeded, status, data, errors)
        {
        }

        public static Result Success()
        {
            return new Result(true, ResultStatus.Success, default, default);
        }

        public static Result<TData> Success<TData>(TData data)
        {
            return new Result<TData>(true, ResultStatus.Success, data, default);
        }

        public static Result BadRequest(IDictionary<string, string[]> errors)
        {
            return new Result(false, ResultStatus.BadRequest, default, errors);
        }

        public static Result<TData> BadRequest<TData>(IDictionary<string, string[]> errors)
        {
            return new Result<TData>(false, ResultStatus.BadRequest, default, errors);
        }

        public static Result Forbidden()
        {
            return new Result(false, ResultStatus.Forbidden, default, null);
        }

        public static Result<TData> Forbidden<TData>()
        {
            return new Result<TData>(false, ResultStatus.Forbidden, default, null);
        }

        public static Result NotFound(string name, object key)
        {
            var errors = new Dictionary<string, string[]>
            {
                {"Source not found", new[] {$"Entity \"{name}\" ({key}) was not found."}}
            };

            return new Result(false, ResultStatus.NotFound, default, errors);
        }

        public static Result NotFound(string message)
        {
            var errors = new Dictionary<string, string[]>
            {
                {"Source not found", new[] {message}}
            };

            return new Result(false, ResultStatus.NotFound, default, errors);
        }

        public static Result<TData> NotFound<TData>(string name, object key)
        {
            var errors = new Dictionary<string, string[]>
            {
                {"Source not found", new[] {$"Entity \"{name}\" ({key}) was not found."}}
            };

            return new Result<TData>(false, ResultStatus.NotFound, default, errors);
        }

        public static Result<TData> NotFound<TData>(string message)
        {
            var errors = new Dictionary<string, string[]>
            {
                {"Source not found", new[] {message}}
            };
            
            return new Result<TData>(false, ResultStatus.NotFound, default, errors);
        }

        public static Result Unauthorized(IDictionary<string, string[]> errors)
        {
            return new Result(false, ResultStatus.Unauthorized, default, errors);
        }

        public static Result<TData> Unauthorized<TData>(IDictionary<string, string[]> errors)
        {
            return new Result<TData>(false, ResultStatus.Unauthorized, default, errors);
        }
    }
}