using Newtonsoft.Json;

namespace Application.TemplateEngines.Commands.Renders
{
    public class RenderResponse
    {
        public string Content { get; set; }
    }

    public class RenderModel
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("render-id")]
        public string RenderId { get; set; }

        [JsonProperty("render-data")]
        public object RenderData { get; set; }

        [JsonProperty("render-data-for-log")]
        public object RenderDataForLog { get; set; }
    }
}
