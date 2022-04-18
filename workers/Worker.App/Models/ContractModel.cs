using Worker.App.Domain.Enums;
using static Worker.App.Models.StartRequest;

namespace Worker.App.Models;

public class ContractModel
{
    public ContractModel()
    {
        Documents = new List<ApproveOrderDocument>();
    }

    public StartRequest StartRequest { get; set; }
    public StartFormRequest StartFormRequest { get; set; }
    public Form FormType { get; set; }

    public Guid InstanceId { get; set; }
    public bool Device { get; set; }

    public bool Approved { get; set; }
    public bool Completed { get; set; }
    public bool IsProcess { get; set; }
    public bool RetryEnd { get; set; }
    public int Limit { get; set; }

    /// <summary>
    /// Zeebe is also controlled from the "Limit" parameter
    /// </summary>
    public int MaxRetryCount { get; set; }
    /// <summary>
    /// Timer Definition "Retry" : PT1M
    /// </summary>
    public string RetryFrequence { get; set; }
    /// <summary>
    /// Timer Definition "Timeout" : PT15M
    /// </summary>
    public string ExpireInMinutes { get; set; }


    public ApproveOrderDocument Document { get; set; }
    public List<ApproveOrderDocument> Documents { get; set; }
}

public class ApproveOrderDocument
{
    public string DocumentId { get; set; }
    public string ActionId { get; set; }
}