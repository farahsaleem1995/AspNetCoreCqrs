using AspCqrs.Api.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AspCqrs.Api.Controllers
{
    [ApiController]
    [ApiExceptionFilter]
    [ApiOkResponseFilter]
    public class ApiBaseController : Controller
    {
        private ISender _mediator;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}