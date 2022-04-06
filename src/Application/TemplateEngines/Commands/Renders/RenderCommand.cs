using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Application.TemplateEngines.Commands.Renders;

public class RenderCommand : IRequest<Response<RenderResponse>>
{
    public string formId { get; set; }
    public string content { get; set; }
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
        var form = _context.FormDefinitions.FirstOrDefault(x => x.FormDefinitionId == request.formId);
        if (form == null)
            return new Response<RenderResponse>();

        var postUrl = ""; // "http://20.126.170.150:5000/Template/Render";
        var name = form.TemplateName;
        var renderId = Guid.NewGuid().ToString();

        string jsonData = "{" +
                          "\"name\": \"Ugur\"," +
                          "\"render-id\": " + Guid.NewGuid().ToString() + "," +
                          "\"render-data\": " + request.content + "," +
                          "\"render-data-for-log\":  " + request.content + "," +
        "}";

        var jsonReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(request.content);
        var restClient = new RestClient();
        var restRequest = new RestRequest()
        {
            Resource = postUrl,
            Method = Method.Post
        };
        restRequest.AddHeader("Content-Type", "application/json");
        restRequest.AddHeader("Accept", "application/json");
        restRequest.AddObject(jsonData);

        var response = await restClient.ExecutePostAsync(restRequest);
        var c = response.Content;

        //var restClient = new RestClient();
        //var restRequest = new RestRequest()
        //{
        //    Resource = postUrl,
        //    Method = Method.Post
        //};
        //restRequest.AddHeader("Content-Type", "application/json");
        //restRequest.AddHeader("Accept", "application/json");

        //JObject json = JObject.Parse(request.content);

        //restRequest.AddParameter("name", name, ParameterType.RequestBody);
        //restRequest.AddParameter("render-id", renderId, ParameterType.RequestBody);
        //restRequest.AddParameter("render-data", json, ParameterType.RequestBody);
        //restRequest.AddParameter("render-data-for-log", json, ParameterType.RequestBody);

        //var response = await restClient.ExecutePostAsync(restRequest);
        //var c = response.Content;



        //var client = new RestClient("http://20.126.170.150:5000");
        //var restRequest = new RestRequest("Template/Render", Method.Post);
        //restRequest.AddHeader("Content-Type", "application/json");
        //restRequest.RequestFormat = DataFormat.Json;

        //restRequest.AddParameter("name", name, ParameterType.RequestBody);
        //restRequest.AddParameter("render-id", renderId, ParameterType.RequestBody);
        //restRequest.AddParameter("render-data", request.content, ParameterType.RequestBody);
        //restRequest.AddParameter("render-data-for-log", request.content, ParameterType.RequestBody);

        //var response = await client.ExecuteAsync(restRequest);
        //var d = response.Content;

        //var model = new RenderModel
        //{
        //    name = name,
        //    RenderId = renderId,
        //    RenderData = request.content,
        //    RenderDataForLog = request.content
        //};
        //var client = new RestClient("http://20.126.170.150:5000");
        //var restRequest = new RestRequest("/Template/Render", Method.Post);
        //restRequest.AddHeader("Content-Type", "application/json");
        //restRequest.AddJsonBody(model);
        //var response = await client.ExecuteAsync(restRequest);


        //var client = new RestClient("http://20.126.170.150:5000");
        //var restRequest = new RestRequest("/Template/Render", Method.Post);
        //restRequest.AddHeader("Content-Type", "application/json");
        //var body = @"{
        //" + "\n" +
        //@"  ""name"": ""Ugur"",
        //" + "\n" +
        //@"  ""render-id"": ""ca3be60f-4de6-47f4-9ab3-58a0d4425f32"",
        //" + "\n" +
        //@"  ""render-data"": { ""name"": ""ugur"" },
        //" + "\n" +
        //@"  ""render-data-for-log"": {""name"":""ugur""}
        //" + "\n" +
        //@"} ";
        //restRequest.AddJsonBody(body);
        //var response = await client.ExecuteAsync(restRequest);

        return Response<RenderResponse>.Success(new RenderResponse { }, 200);
    }
}
