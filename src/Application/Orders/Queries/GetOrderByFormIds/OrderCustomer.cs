namespace Application.Orders.Queries.GetOrderByFormIds;


public class OrderCustomer
{
    public long CitizenshipNumber { get; set; }
    public string First { get; set; }
    public string Last { get; set; }
    public UInt64 CustomerNumber { get; set; }
    public string BranchCode { get; set; }
    public string BusinessLine { get; set; }
}