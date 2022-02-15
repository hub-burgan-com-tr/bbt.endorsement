using Infrastructure.ZeebeServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        private ISender _mediator = null!;
        private IZeebeService _zeebeService = null!;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
        protected IZeebeService ZeebeService => _zeebeService ??= HttpContext.RequestServices.GetRequiredService<IZeebeService>();
    }
}
