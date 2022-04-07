using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Html;
using RestSharp;
using System.Web;

namespace Application.TemplateEngines.Commands.Renders;

public class RenderCommand : IRequest<Response<RenderResponse>>
{
    public string FormId { get; set; }
    public string Content { get; set; }
}

public class RenderCommandHandler : IRequestHandler<RenderCommand, Response<RenderResponse>>
{
    private readonly IApplicationDbContext _context;

    public RenderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Response<RenderResponse>> Handle(RenderCommand request, CancellationToken cancellationToken)
    {
        var form = _context.FormDefinitions.FirstOrDefault(x => x.FormDefinitionId == request.FormId);
        if (form == null)
            return new Response<RenderResponse>();

        string jsonData = @"{" +
                          "\"name\":"  + "\"" + form.TemplateName + "\"" + "," +
                          "\"render-id\":" + "\"" + Guid.NewGuid().ToString() + "\"" + "," +
                          "\"render-data\": " + request.Content + "," +
                          "\"render-data-for-log\":  " + request.Content +
        "}";

        var restClient = new RestClient("http://20.126.170.150:5000");
        var restRequest = new RestRequest("/Template/Render", Method.Post);
        restRequest.AddHeader("Content-Type", "application/json");
        restRequest.AddHeader("Accept", "application/json");
        restRequest.AddStringBody(jsonData, DataFormat.Json);
        var response = await restClient.ExecutePostAsync(restRequest);

        var content = response.Content;
        var html = content;

        html = html.Replace(@"\""", String.Empty);
        html = html.Replace("\"", String.Empty);

        return Response<RenderResponse>.Success(new RenderResponse { Content = html }, 200);
    }
}
