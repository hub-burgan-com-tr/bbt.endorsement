namespace Worker.App.Models
{
    public class ContractModel
    {
        public ContractModel()
        {
            Documents = new List<object>();
        }

        public StartRequest StartRequest { get; set; }
        public StartFormRequest StartFormRequest { get; set; }

        public Guid InstanceId { get; set; }
        public bool Device { get; set; }

        public bool Approved { get; set; }
        public bool Completed { get; set; }
        public bool IsProcess { get; set; }
        public bool RetryEnd { get; set; }
        public int Limit { get; set; }


        public object Document { get; set; }
        public List<object> Documents { get; set; }
    }

}
