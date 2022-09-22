using Newtonsoft.Json;

namespace Application.BbtInternals.Models
{
    public partial class PersonResponse
    {
        [JsonProperty("customerNumber")]
        public int CustomerNumber { get; set; }
        [JsonProperty("name")]
        public Name Name { get; set; }
        [JsonProperty("citizenshipNumber")]
        public string CitizenshipNumber { get; set; }
        [JsonProperty("taxNo")]
        public string TaxNo { get; set; }
        [JsonProperty("isStaff")]
        public bool IsStaff { get; set; }
        [JsonProperty("gsmPhones")]
        public GsmPhones GsmPhones { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("businessLine")]
        public string BusinessLine { get; set; }
        [JsonProperty("devices")]
        public Device[] Devices { get; set; }

        [JsonProperty("authory")]
        public AuthoryModel Authory { get; set; }
        [JsonProperty("identityNumber")]
        public AuthoryModel IdentityNumber { get; set; }
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
        public long Country { get; set; }

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

    public class AuthoryModel
    {
        [JsonProperty("isreadyformcreator")]
        public bool IsReadyFormCreator { get; set; } // Form ile Emir Oluşturma
        [JsonProperty("isnewformcreator")]
        public bool IsNewFormCreator { get; set; } //Yeni Onay Emri Oluşturma
        [JsonProperty("isformreader")]
        public bool IsFormReader { get; set; } // Tüm Onay Emirlerini İzleyebilir
        [JsonProperty("isbranchformreader")]
        public bool IsBranchFormReader { get; set; } //Farklı Şube Onay İsteme
        [JsonProperty("isbranchapproval")]
        public bool IsBranchApproval { get; set; } //Farklı Şube Onay Listeleme
    }

}
