using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.OrderForms.Queries.GetTagsFormName
{
    public class GetTagsFormNameQuery : IRequest<Response<List<GetTagsFormDto>>>
    {
        public string FormDefinitionTagId { get; set; }
    }

    public class GetTagsFormQueryHandler : IRequestHandler<GetTagsFormNameQuery, Response<List<GetTagsFormDto>>>
    {
        private IApplicationDbContext _context;

        public GetTagsFormQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<GetTagsFormDto>>> Handle(GetTagsFormNameQuery request, CancellationToken cancellationToken)
        {
            var response = _context.FormDefinitionTagMaps.Include(x=>x.FormDefinition).Where(x=>x.FormDefinitionTagId==request.FormDefinitionTagId).Select(x => new GetTagsFormDto { FormDefinitionId = x.FormDefinitionId, Name = x.FormDefinition.Name }).OrderBy(x => x.Name).ToList();
            return Response<List<GetTagsFormDto>>.Success(response, 200);
        }
    }

}
