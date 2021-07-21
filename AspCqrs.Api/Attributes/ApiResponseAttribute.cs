using System;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Enums;
using AspCqrs.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspCqrs.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiResponseAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executedContext = await next();

            if (executedContext.Result is ObjectResult objectResult)
            {
                var resultType = objectResult.Value?.GetType().GetGenericTypeDefinition();
                var resultTypeArgs = objectResult.Value?.GetType().GetGenericArguments();

                if (resultType == typeof(Result<>))
                {
                    var statusObject = resultType.MakeGenericType(resultTypeArgs).GetProperty("Status")?.GetValue(objectResult.Value, null);
                    
                    var statusParsResult = Enum.TryParse<ResultStatus>(statusObject?.ToString(), out var status);
                    
                    if (!statusParsResult) throw new ArgumentOutOfRangeException();
                    
                    executedContext.Result = status switch
                    {
                        ResultStatus.BadRequest => new BadRequestObjectResult(objectResult.Value),
                        ResultStatus.Forbidden => new ForbidResult(),
                        ResultStatus.NotFound => new NotFoundObjectResult(objectResult.Value),
                        ResultStatus.Success => new OkObjectResult(objectResult.Value),
                        ResultStatus.Unauthorized => new UnauthorizedObjectResult(objectResult.Value),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }
            }
        }
    }
}