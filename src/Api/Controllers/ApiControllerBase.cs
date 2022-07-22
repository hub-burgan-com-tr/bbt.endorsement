using Application.Common.Interfaces;
using AspNet.Security.OAuth.Validation;
using Infrastructure.ZeebeServices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
