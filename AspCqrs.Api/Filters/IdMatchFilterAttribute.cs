using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AspCqrs.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspCqrs.Api.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class IdMatchFilterAttribute : ValidationAttribute, IAsyncActionFilter
    {
        private readonly Type _argumentType;
        private readonly string _argumentPropertyName;
        private readonly string _routeParameterName;

        public IdMatchFilterAttribute(Type argumentType, string argumentPropertyName, string routeParameterName = "id")
        {
            _argumentType = argumentType;
            _argumentPropertyName = argumentPropertyName;
            _routeParameterName = routeParameterName;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var roueIdValue = context.HttpContext.Request.RouteValues[_routeParameterName];

            var body = context.ActionArguments.Values.FirstOrDefault(v => v?.GetType() == _argumentType);

            var bodyIdValue = _argumentType.GetProperty(_argumentPropertyName)?.GetValue(body, null);

            if (roueIdValue != bodyIdValue)
            {
                context.Result = new BadRequestObjectResult(new ApiResponse<object>
                {
                    Succeeded = false,
                    Errors = new Dictionary<string, string[]>
                    {
                        {
                            "IdMismatch",
                            new[]
                            {
                                $"Route ID value ({roueIdValue}) does not match body ID Property value ({bodyIdValue})."
                            }
                        }
                    },
                    Data = null
                });
            }
            else
            {
                await next();
            }
        }
    }
}