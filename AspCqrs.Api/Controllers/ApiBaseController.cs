using AspCqrs.Api.Attributes;
using AspCqrs.Api.Filters;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspCqrs.Api.Controllers
{
    [ApiController]
    [ApiExceptionFilter]
    [ApiResponse]
    public class ApiBaseController : Controller
    {
        protected readonly IMediator Mediator;
        protected readonly IMapper Mapper;

        public ApiBaseController(IMediator mediator, IMapper mapper)
        {
            Mediator = mediator;
            Mapper = mapper;
        }
    }
}