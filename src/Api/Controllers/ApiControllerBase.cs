﻿using Api.Extensions;
using Application.Common.Interfaces;
using AspNet.Security.OAuth.Validation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [UserAttribute]
    public class ApiControllerBase : ControllerBase
    {
        private ISender _mediator = null!;
        private IZeebeService _zeebeService = null!;
       // private IHttpContextAccessor _httpContextAccessor;


        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
        protected IZeebeService ZeebeService => _zeebeService ??= HttpContext.RequestServices.GetRequiredService<IZeebeService>();
        //protected IHttpContextAccessor HttpContextAccessor => _httpContextAccessor ??= HttpContextAccessor;
    }
}
