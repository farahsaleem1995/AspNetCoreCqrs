using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AspCqrs.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspCqrs.Api.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MatchRouteAndBodyFilterAttribute : ValidationAttribute, IAsyncActionFilter
    {
        private readonly string _bodyModelPropertyName;
        private readonly string _routeParameterName;

        public MatchRouteAndBodyFilterAttribute(string routeParameterName)
        {
            _routeParameterName = routeParameterName.ToLower();
            _bodyModelPropertyName = routeParameterName.ToLower();
        }

        public MatchRouteAndBodyFilterAttribute(string routeParameterName, string bodyModelPropertyName)
        {
            _routeParameterName = routeParameterName.ToLower();
            _bodyModelPropertyName = bodyModelPropertyName.ToLower();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var (routeKey, routeValue) =
                context.HttpContext.Request.RouteValues.FirstOrDefault(v => v.Key.ToLower() == _routeParameterName);

            var body = context.ActionArguments.Values.FirstOrDefault(v => v is IRequest);
            var bodyType = body?.GetType();
            var bodyKey = bodyType?.GetProperties().FirstOrDefault(p => p.Name.ToLower() == _bodyModelPropertyName)?.Name;
            var bodyValue = bodyType?.GetProperty(bodyKey ?? throw new ArgumentException($"Property '{_bodyModelPropertyName}' does not exist in Type '{bodyType}'"))?.GetValue(body, null);

            if (!routeValue.Equals(bodyValue?.ToString()))
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
                                $"Route parameter '{routeKey}' value ({routeValue}) does not match body property '{bodyKey}' value ({bodyValue})."
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