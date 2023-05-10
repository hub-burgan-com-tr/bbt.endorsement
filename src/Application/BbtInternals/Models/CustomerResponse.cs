using Newtonsoft.Json;

namespace Application.BbtInternals.Models;

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
    public CustomerList()
    {
        Authory = new AuthoryModel();
    }

    [JsonProperty("customerNumber")]
    public UInt64 CustomerNumber { get; set; }
    [JsonProperty("name")]
    public Name Name { get; set; }
    [JsonProperty("citizenshipNumber")]
    public string CitizenshipNumber { get; set; }
    [JsonProperty("taxNo")]
    public string TaxNo { get; set; }
    [JsonProperty("isStaff")]
    public bool IsStaff { get; set; }
    [JsonProperty("gsmPhone")]
    public GsmPhones GsmPhone { get; set; }
    [JsonProperty("email")]
    public string Email { get; set; }
    [JsonProperty("businessLine")]
    public string BusinessLine { get; set; }
    [JsonProperty("device")]
    public Device[] Devices { get; set; }
    [JsonProperty("identityNumber")]
    public string IdentityNumber { get; set; }
    [JsonProperty("branchCode")]
    public string BranchCode { get; set; }
    [JsonProperty("recordStatus")]
    public string RecordStatus { get; set; }
    public AuthoryModel Authory { get; set; }
}