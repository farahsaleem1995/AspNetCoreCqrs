using System;
using System.Threading.Tasks;
using AspCqrs.Api.Models;
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
                var apiResponseTypeArgs = objectResult.Value?.GetType();
                var apiResponseType = typeof(ApiResponse<>).MakeGenericType(apiResponseTypeArgs ?? throw new ArgumentException("Null type"));

                var response = Activator.CreateInstance(apiResponseType);
                
                apiResponseType.GetProperty("Success")?.SetValue(response, true);
                apiResponseType.GetProperty("Errors")?.SetValue(response, null);
                apiResponseType.GetProperty("Data")?.SetValue(response, objectResult.Value);
                
                executedContext.Result = new OkObjectResult(response);
            }
        }
    }
}