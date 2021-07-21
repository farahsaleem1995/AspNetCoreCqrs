using System;
using System.Collections.Generic;
using System.Linq;
using AspCqrs.Application.Common.Enums;
using AspCqrs.Application.Common.Exceptions;
using AspCqrs.Application.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspCqrs.Api.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        public ApiExceptionFilterAttribute()
        {
            // Register known exception types and handlers.
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                {typeof(ValidationException), HandleValidationException},
                {typeof(NotFoundException), HandleNotFoundException},
                {typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException},
                {typeof(ForbiddenAccessException), HandleForbiddenAccessException},
            };
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);

            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            var type = context.Exception.GetType();

            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }

            if (!context.ModelState.IsValid)
            {
                HandleInvalidModelStateException(context);
                return;
            }

            HandleUnknownException(context);
        }

        private static void HandleValidationException(ExceptionContext context)
        {
            var exception = context.Exception as ValidationException;

            context.Result =
                new BadRequestObjectResult(new Result(false, ResultStatus.NotFound, null, exception?.Errors));

            context.ExceptionHandled = true;
        }

        private static void HandleInvalidModelStateException(ExceptionContext context)
        {
            var details = new ValidationProblemDetails(context.ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };
            
            var result = new Result(false, ResultStatus.NotFound, null, details.Errors);

            context.Result = new BadRequestObjectResult(result);

            context.ExceptionHandled = true;
        }

        private static void HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundException;

            context.Result =
                new NotFoundObjectResult(new Result(false, ResultStatus.NotFound, null, new Dictionary<string, string[]>
                {
                    {"Source not found", new[] {exception?.Message}}
                }));

            context.ExceptionHandled = true;
        }

        private static void HandleUnauthorizedAccessException(ExceptionContext context)
        {
            var exception = context.Exception as UnauthorizedAccessException;
            var result = new Result(false, ResultStatus.NotFound, null,
                new Dictionary<string, string[]>
                {
                    {"Unauthorized", new[] {exception?.Message}}
                });

            context.Result = new ObjectResult(result)
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };

            context.ExceptionHandled = true;
        }

        private static void HandleForbiddenAccessException(ExceptionContext context)
        {
            var result = new Result(false, ResultStatus.Forbidden, null, null);

            context.Result = new ObjectResult(result)
            {
                StatusCode = StatusCodes.Status403Forbidden
            };

            context.ExceptionHandled = true;
        }

        private static void HandleUnknownException(ExceptionContext context)
        {
            var result = new Result(false, ResultStatus.NotFound, null,
                new Dictionary<string, string[]>
                {
                    {"Unknown", new[] {"An error occurred while processing your request."}}
                });

            context.Result = new ObjectResult(result)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}