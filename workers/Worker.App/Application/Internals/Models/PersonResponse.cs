﻿using Newtonsoft.Json;

namespace Worker.App.Application.Internals.Models
{
    public partial class PersonResponse
    {
        [JsonProperty("customerNumber")]
        public int CustomerNumber { get; set; }

        [JsonProperty("citizenshipNumber")]
        public long CitizenshipNumber { get; set; }

        [JsonProperty("name")]
        public Name Name { get; set; }

        [JsonProperty("gsmPhone")]
        public GsmPhones GsmPhone { get; set; }

        [JsonProperty("emails")]
        public string[] Emails { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

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

    public partial class GsmPhones
    {
        [JsonProperty("country")]
        public int ? Country { get; set; }

        [JsonProperty("prefix")]
        public int ? Prefix { get; set; }

        [JsonProperty("number")]
        public int ? Number { get; set; }
    }

    public partial class Name
    {
        [JsonProperty("first")]
        public string First { get; set; }

        [JsonProperty("last")]
        public string Last { get; set; }
    }
}
