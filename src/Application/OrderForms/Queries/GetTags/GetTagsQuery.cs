using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.OrderForms.Queries.GetTags
{
    public class GetTagsQuery : IRequest<Response<List<GetTagsDto>>>
    {
    }
    public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, Response<List<GetTagsDto>>>
    {
        private IApplicationDbContext _context;
        public GetTagsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<GetTagsDto>>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            var response = _context.FormDefinitionTags
                .Select(x => new GetTagsDto { FormDefinitionTagId = x.FormDefinitionTagId, Tag = x.Tag,IsProcessNo=x.IsProcessNo }).OrderBy(x => x.Tag).ToList();
            return Response<List<GetTagsDto>>.Success(response, 200);
        }

    }


}
