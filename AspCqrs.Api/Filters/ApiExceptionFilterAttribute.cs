using System;
using System.Collections.Generic;
using AspCqrs.Api.Models;
using AspCqrs.Application.Common.Exceptions;
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
                {typeof(UnauthorizedRequestException), HandleUnauthorizedRequestException},
                {typeof(ForbiddenRequestException), HandleForbiddenRequestException},
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

            context.Result = new BadRequestObjectResult(new ApiResponse<object>
            {
                Succeeded = false,
                Errors = exception?.Errors,
                Data = null
            });

            context.ExceptionHandled = true;
        }

        private static void HandleInvalidModelStateException(ExceptionContext context)
        {
            var details = new ValidationProblemDetails(context.ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            var result = new ApiResponse<object>
            {
                Succeeded = false,
                Errors = details.Errors,
                Data = null
            };

            context.Result = new BadRequestObjectResult(result);

            context.ExceptionHandled = true;
        }

        private static void HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundException;

            context.Result = new NotFoundObjectResult(new ApiResponse<object>
            {
                Succeeded = false,
                Errors = new Dictionary<string, string[]>
                {
                    {"SourceNotFound", new[] {exception?.Message}}
                },
                Data = null
            });

            context.ExceptionHandled = true;
        }

        private static void HandleUnauthorizedRequestException(ExceptionContext context)
        {
            var exception = context.Exception as UnauthorizedRequestException;
            var result = new ApiResponse<object>
            {
                Succeeded = false,
                Errors = exception?.Errors,
                Data = null
            };

            context.Result = new ObjectResult(result)
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };

            context.ExceptionHandled = true;
        }

        private static void HandleForbiddenRequestException(ExceptionContext context)
        {
            context.Result = new ForbidResult();

            context.ExceptionHandled = true;
        }

        private static void HandleUnknownException(ExceptionContext context)
        {
            var result = new ApiResponse<object>
            {
                Succeeded = false,
                Errors = new Dictionary<string, string[]>
                {
                    {"Unknown", new[] {"An error occurred while processing your request."}}
                },
                Data = null
            };

            context.Result = new ObjectResult(result)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}