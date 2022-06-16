using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;


namespace Application.OrderForms.Queries.GetForms
{
    public class GetFormQuery : IRequest<Response<List<GetFormDto>>>
    {
    }
    public class GetFormQueryHandler : IRequestHandler<GetFormQuery, Response<List<GetFormDto>>>
    {
        private IApplicationDbContext _context;

        public GetFormQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<GetFormDto>>> Handle(GetFormQuery request, CancellationToken cancellationToken)
        {
            var response = _context.FormDefinitions.Select(x=>new GetFormDto { FormDefinitionId=x.FormDefinitionId,Name=x.Name}).OrderBy(x=>x.Name).ToList();
            return Response<List<GetFormDto>>.Success(response, 200);
        }
    }

}
