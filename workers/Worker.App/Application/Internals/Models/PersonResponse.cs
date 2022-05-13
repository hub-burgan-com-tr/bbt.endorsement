using Newtonsoft.Json;

namespace Worker.App.Application.Internals.Models
{
    public partial class PersonResponse
    {
        [JsonProperty("clientNumber")]
        public int ClientNumber { get; set; }

        [JsonProperty("citizenshipNumber")]
        public long CitizenshipNumber { get; set; }

        [JsonProperty("name")]
        public Name Name { get; set; }

        [JsonProperty("gsmPhones")]
        public GsmPhone[] GsmPhones { get; set; }

        [JsonProperty("emails")]
        public string[] Emails { get; set; }

        [JsonProperty("devices")]
        public Device[] Devices { get; set; }
    }

    public partial class Device
    {
        [JsonProperty("deviceId")]
        public Guid DeviceId { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }

    public partial class GsmPhone
    {
        [JsonProperty("county")]
        public long County { get; set; }

        [JsonProperty("prefix")]
        public long Prefix { get; set; }

        [JsonProperty("number")]
        public long Number { get; set; }
    }

    public partial class Name
    {
        [JsonProperty("first")]
        public string First { get; set; }

        [JsonProperty("last")]
        public string Last { get; set; }
    }
}
