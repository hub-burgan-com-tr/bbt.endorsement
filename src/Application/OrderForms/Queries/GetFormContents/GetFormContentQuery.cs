using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.OrderForms.Queries.GetFormContents
{
    public class GetFormContentQuery : IRequest<Response<GetFormContentDto>>
    {
        public string FormDefinitionId { get; set; }
    }
    public class GetFormContentQueryHandler : IRequestHandler<GetFormContentQuery, Response<GetFormContentDto>>
    {
        private IApplicationDbContext _context;

        public GetFormContentQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<GetFormContentDto>> Handle(GetFormContentQuery request, CancellationToken cancellationToken)
        {
            var response = _context.FormDefinitions.Where(x => x.FormDefinitionId == request.FormDefinitionId).Select(x=>new GetFormContentDto { Content=x.Label,Title=x.Name,Source=x.Source}).FirstOrDefault();
            return Response<GetFormContentDto>.Success(response, 200);
        }
    }

}
