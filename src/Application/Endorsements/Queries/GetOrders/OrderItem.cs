namespace Application.Endorsements.Queries.GetOrders;


public class OrderItem
{
    public Guid Id { get; set; }

    public long Customer { get; set; }
    public long Approver { get; set; }
    public ReferenceClass Reference { get; set; }

    public class ReferenceClass
    {
        public string Process { get; set; }
        public string State { get; set; }
        public Guid Id { get; set; }
    }
    public StatusType Status { get; set; }
    public enum StatusType { Completed, InProgress, Canceled, Halted }

    public DocumentClass[] Documents { get; set; }
    public class DocumentClass
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public StatusType Status { get; set; }
        public enum StatusType { Approved, InProgress, Rejected }
    }
}