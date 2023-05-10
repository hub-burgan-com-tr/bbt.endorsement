using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderForms.Queries.GetFormInformations
{
    public class GetFormInformationQuery : IRequest<Response<List<GetFormInformationDto>>>
    {
    }

    public class GetFormInformationQueryHandler : IRequestHandler<GetFormInformationQuery, Response<List<GetFormInformationDto>>>
    {
        private IApplicationDbContext _context;

        public GetFormInformationQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<GetFormInformationDto>>> Handle(GetFormInformationQuery request, CancellationToken cancellationToken)
        {
            var response = _context.FormDefinitions.Include(x=>x.FormDefinitionTagMaps).ThenInclude(x=>x.FormDefinitionTag).OrderByDescending(x=>x.Created).Select(x => new GetFormInformationDto {FormDefinitionId=x.FormDefinitionId,Tag=x.FormDefinitionTagMaps.FirstOrDefault().FormDefinitionTag.Tag,Name=x.Name, TemplateName = x.TemplateName, Type = x.Type,Source=x.Source,MaxRetryCount=x.MaxRetryCount,ExpireInMinutes=x.ExpireInMinutes,RetryFrequence=x.RetryFrequence }).ToList();
            return Response<List<GetFormInformationDto>>.Success(response, 200);
        }
    }

}
