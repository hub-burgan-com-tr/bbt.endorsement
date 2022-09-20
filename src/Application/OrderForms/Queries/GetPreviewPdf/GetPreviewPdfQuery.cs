using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Enums;
using MediatR;

namespace Application.OrderForms.Queries.GetPreviewPdf
{
    public class GetPreviewPdfQuery : IRequest<Response<GetPreviewPdfDto>>
    {
        public string FormId { get; set; }
        public string Content { get; set; }


        public class GetPreviewPdfQueryHandler : IRequestHandler<GetPreviewPdfQuery, Response<GetPreviewPdfDto>>
        {
            private IApplicationDbContext _context;
            private readonly ITemplateEngineService _templateEngineService;

            public GetPreviewPdfQueryHandler(IApplicationDbContext context, ITemplateEngineService templateEngineService)
            {
                _context = context;
                _templateEngineService = templateEngineService;
            }
            public async Task<Response<GetPreviewPdfDto>> Handle(GetPreviewPdfQuery request, CancellationToken cancellationToken)
            {
                var form = _context.FormDefinitions.FirstOrDefault(x => x.FormDefinitionId == request.FormId);
                if (form == null)
                    return new Response<GetPreviewPdfDto>();
                var content = request.Content;
                content = content.Replace("\"sigortaEttirenIleSigortaliAyniKisidir\":true", "\"sigortaEttirenIleSigortaliAyniKisidir\":\"EVET\"");
                content = content.Replace("\"sigortaEttirenIleSigortaliAyniKisidir\":false", "\"sigortaEttirenIleSigortaliAyniKisidir\":\"HAYIR\"");
                content = content.Replace("true", "\"X\"");
                content = content.Replace("True", "\"X\"");
                content = content.Replace("false", "\"\"");
                content = content.Replace("False", "\"\"");
                var html = "";
                if (form.Type == ContentType.PDF.ToString())
                {
                    var response = await _templateEngineService.PdfRender(form.TemplateName, content);
                    html = response.Data;
                }

                return Response<GetPreviewPdfDto>.Success(new GetPreviewPdfDto { Content = html}, 200);
            }
        }

    }
}
