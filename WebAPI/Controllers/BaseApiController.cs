using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private readonly IMediator _mediator;
        protected IMediator? Mediator => _mediator ?? HttpContext.RequestServices.GetService<IMediator>();
    }
}