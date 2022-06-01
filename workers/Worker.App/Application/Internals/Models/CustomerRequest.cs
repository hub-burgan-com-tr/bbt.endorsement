using Newtonsoft.Json;

namespace Worker.App.Application.Internals.Models;

public class CustomerRequest
{
    public CustomerName name { get; set; }
    public string identityNumber { get; set; }
    public int customerNumber { get; set; }
    public int page { get; set; } = 1;
    public int size { get; set; } = 10;
}

public class CustomerName
{
    public string first { get; set; }
    public string last { get; set; }
}

public class CustomerResponse
{
    [JsonProperty("recordCount")]
    public int RecordCount { get; set; }
    [JsonProperty("returnCode")]
    public int ReturnCode { get; set; }
    [JsonProperty("returnDescription")]
    public string ReturnDescription { get; set; }
    [JsonProperty("customerList")]
    public List<CustomerList> CustomerList { get; set; }
}


public class CustomerList
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
    [JsonProperty("gsmPhone")]
    public GsmPhone GsmPhone { get; set; }
    [JsonProperty("email")]
    public string Email { get; set; }
    [JsonProperty("businessLine")]
    public string BusinessLine { get; set; }
    [JsonProperty("device")]
    public Device[] Devices { get; set; }
    [JsonProperty("identityNumber")]
    public string IdentityNumber { get; set; }
}
