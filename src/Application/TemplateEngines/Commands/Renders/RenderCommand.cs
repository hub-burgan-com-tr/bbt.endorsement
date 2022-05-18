using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Enums;
using MediatR;
using System.Text;

namespace Application.TemplateEngines.Commands.Renders;

public class RenderCommand : IRequest<Response<RenderResponse>>
{
    public string FormId { get; set; }
    public string Content { get; set; }
}

public class RenderCommandHandler : IRequestHandler<RenderCommand, Response<RenderResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ITemplateEngineService _templateEngineService;

    public RenderCommandHandler(IApplicationDbContext context, ITemplateEngineService templateEngineService)
    {
        _context = context;
        _templateEngineService = templateEngineService;
    }

    public async Task<Response<RenderResponse>> Handle(RenderCommand request, CancellationToken cancellationToken)
    {
        var form = _context.FormDefinitions.FirstOrDefault(x => x.FormDefinitionId == request.FormId);
        if (form == null)
            return new Response<RenderResponse>();

        var content = request.Content.Replace("true", "\"X\"");
        content = content.Replace("True", "\"X\"");
        content = content.Replace("false", "\"\"");
        content = content.Replace("False", "\"\"");

        var html = "";
        if(form.Type == ContentType.HTML.ToString())
        {
            var response = await _templateEngineService.HtmlRender(form.TemplateName, content);

            var plainTextBytes = Encoding.UTF8.GetBytes(response.Data);
            var encode = Convert.ToBase64String(plainTextBytes);

            //var base64EncodedBytes = System.Convert.FromBase64String(encode);
            //var decode = Encoding.UTF8.GetString(base64EncodedBytes);

            var data = "data:text/html;base64," + encode;

            html = data;
        }
        else if(form.Type == ContentType.PDF.ToString())
        {
            var response = await _templateEngineService.PdfRender(form.TemplateName, content);
            html = response.Data;
        }

        return Response<RenderResponse>.Success(new RenderResponse { Content = html }, 200);
    }
}
