using Application.Common.Interfaces;
using Application.Common.Models;
using Application.SSOIntegrationService.Models.Request;
using Application.TemplateEngines.Commands.Renders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.SSOIntegrationService.Commands
{
    public class SSOIntegrationCommand : IRequest<Response<SSOIntegrationResponse>>
    {
        //public GetAuthorityForUser getAuthorityForUser { get; set; }
        //public SearchUserInfo searchUserInfo { get; set; }
        public string RegisterId { get; set; }
        public string RequestUserName { get; set; }

    }

    public class SSOIntegrationCommandHandler : IRequestHandler<SSOIntegrationCommand, Response<SSOIntegrationResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ISSOIntegrationService _SSOIntegrationService;

        public SSOIntegrationCommandHandler(IApplicationDbContext context, ISSOIntegrationService SSOIntegrationService)
        {
            _context = context;
            _SSOIntegrationService = SSOIntegrationService;
        }

        public async Task<Response<SSOIntegrationResponse>> Handle(SSOIntegrationCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.RequestUserName))
            {
                return Response<SSOIntegrationResponse>.Fail("UserName Bos Olamaz!", 417);
            }
            if (request.RequestUserName.Length < 4)
            {
                return Response<SSOIntegrationResponse>.Fail("UserName 4 karakterden küçük Olamaz!", 417);

            }
            request.RegisterId = Regex.Match(request.RequestUserName, @"\d+").Value;

            var res = new SSOIntegrationResponse();

            var resUserByRegisterId = await _SSOIntegrationService.GetUserByRegisterId(res.RegisterId);
            if (resUserByRegisterId.StatusCode == 200)
            {
                res.UserInfo = resUserByRegisterId.Data;
                var resAuthorityForUser = await _SSOIntegrationService.GetAuthorityForUser("MOBIL_ONAY", "Credentials", res.UserInfo.LoginName);
                res.UserAuthorities= resAuthorityForUser.Data;
            }

            return Response<SSOIntegrationResponse>.Success(res, 200);
        }
    }

}
