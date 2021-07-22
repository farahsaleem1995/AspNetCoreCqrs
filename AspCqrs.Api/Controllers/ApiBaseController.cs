using AspCqrs.Api.Attributes;
using AspCqrs.Api.Filters;
using AspCqrs.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AspCqrs.Api.Controllers
{
    [ApiController]
    [ApiExceptionFilter]
    [ApiResponse]
    public class ApiBaseController : Controller
    {
        private ISender _mediator;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}