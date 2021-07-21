using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspCqrs.Api.Controllers
{
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