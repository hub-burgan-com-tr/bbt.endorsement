using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.OrderForms.Queries.GetFormContents
{
    public class GetFormContentQuery : IRequest<Response<string>>
    {
        public string FormDefinitionId { get; set; }
    }
    public class GetFormContentQueryHandler : IRequestHandler<GetFormContentQuery, Response<string>>
    {
        private IApplicationDbContext _context;

        public GetFormContentQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<string>> Handle(GetFormContentQuery request, CancellationToken cancellationToken)
        {
            var response = _context.FormDefinitions.Where(x => x.FormDefinitionId == request.FormDefinitionId).FirstOrDefault()?.Label;
            return Response<string>.Success(response, 200);
        }
    }

}
