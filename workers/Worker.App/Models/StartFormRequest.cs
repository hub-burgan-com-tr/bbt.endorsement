namespace Worker.App.Models;

public class StartFormRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string FormId { get; set; }
    public string Content { get; set; }
    public OrderReference Reference { get; set; }
    public OrderApprover Approver { get; set; }

    //public class OrderApprover
    //{
    //    public int Type { get; set; }
    //    public string Value { get; set; }
    //    public string NameSurname { get; set; }
    //}

    public class OrderReference
    {
        public string Process { get; set; }
        public string State { get; set; }
        public string ProcessNo { get; set; }
        public CallbackClass Callback { get; set; }
    }

    public class CallbackClass
    {
        public CalbackMode Mode { get; set; }
        public string URL { get; set; }
        public enum CalbackMode { Completed, Verbose }
    }
}