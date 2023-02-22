using Domain.Enums;

namespace Domain.Models;

public class ContractModel
{
    public ContractModel()
    {
        Documents = new List<ApproveOrderDocument>();
        DmsIds = new List<DMSDocumentResponse>();
        Services = new List<string>();
    }

    public StartRequest StartRequest { get; set; }
    public StartFormRequest StartFormRequest { get; set; }
    public OrderPerson Person { get; set; }
    public Form FormType { get; set; }
    public string ContentData { get; set; }

    public string InstanceId { get; set; }
    public bool Device { get; set; }
    public bool IsPersonalMail { get; set; }

    public bool Approved { get; set; }
    public bool Completed { get; set; }
    public bool IsProcess { get; set; } = true;
    public bool RetryEnd { get; set; }
    public int Limit { get; set; }

    public int MaxRetryCount { get; set; }
    public string RetryFrequence { get; set; }
    public string ExpireInMinutes { get; set; }

    public string DependencyFormId { get; set; }
    public bool DependecyRules { get; set; }

    public string Error { get; set; }

    public string[] Urls { get; set; }
    public List<string> Services { get; set; }

    public List<ApproveOrderDocument> Documents { get; set; }
    public List<DMSDocumentResponse> DmsIds { get; set; }
}

public class DMSDocumentResponse
{
    public string DmsRefId { get; set; } // CreateDMSDocument Gelen
    public int? DmsReferenceKey { get; set; }
    public string DmsReferenceName { get; set; }
}

public class ApproveOrderDocument
{
    public string DocumentId { get; set; }
    public string ActionId { get; set; }
}

public class StartRequest
{
    /// <summary>
    /// Unique Id of order. Id is corrolation key of workflow also. 
    /// </summary>
    public string Id { get; set; }
    public string Title { get; set; }

    public OrderConfig Config { get; set; }
    public OrderReference Reference { get; set; }
    public List<OrderDocument> Documents { get; set; }
    public OrderCustomer Approver { get; set; }
}

public class OrderCustomer
{
    public long CitizenshipNumber { get; set; }
    public string First { get; set; }
    public string Last { get; set; }
    public int CustomerNumber { get; set; }
    public string BranchCode { get; set; }
    public string BusinessLine { get; set; }
}

public class OrderPerson
{
    public long CitizenshipNumber { get; set; }
    public string First { get; set; }
    public string Last { get; set; }
    public int CustomerNumber { get; set; }
    public string BranchCode { get; set; }
    public string BusinessLine { get; set; }
    public string Email { get; set; }

    /// <summary>
    /// IsBranchApproval=true ise şubeler tüm müşterileri listeleyecek. IsBranchApproval=false şubeler sadece kendi şube müşterilerini listeleyecek.
    /// </summary>
    public bool IsBranchApproval { get; set; }

    /// <summary>
    /// IsBranchFormReader=true ise şubeler tüm emirleri listeleyecek. IsBranchFormReader=false şubeler sadece kendi şube emirlerini listeleyecek.
    /// </summary>
    public bool IsBranchFormReader { get; set; }
    public bool IsFormReader { get; set; }
    public bool IsNewFormCreator { get; set; }
    public bool IsReadyFormCreator { get; set; }
}
public class OrderDocument
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string FileName { get; set; }
    public int Type { get; set; }
    public string FileType { get; set; }
    public List<DocumentActionClass> Actions { get; set; }

}
public class DocumentActionClass
{
    public int Choice { get; set; }
    public string Title { get; set; }
    public ActionType Type { get; set; }
}
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
}

public class OrderConfig
{
    public int MaxRetryCount { get; set; }
    public int RetryFrequence { get; set; }
    public int ExpireInMinutes { get; set; }
    public bool IsPersonalMail { get; set; }
    public bool Device { get; set; }
    public string NotifyMessageSMS { get; set; }
    public string NotifyMessagePush { get; set; }
    public string RenotifyMessageSMS { get; set; }
    public string RenotifyMessagePush { get; set; }
}

public class StartFormRequest
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string FormId { get; set; }
    public string Content { get; set; }
    public string Source { get; set; }
    public string InsuranceType { get; set; }
    public string FileType { get; set; }

    public string DependencyOrderId { get; set; }
  
    public string DependencyFormId { get; set; }
    public bool DependecyRules { get; set; }=false;
    public OrderReference Reference { get; set; }
    public OrderCustomer Approver { get; set; }
    public OrderConfig OrderConfig { get; set; }
}

public class FormDefinitionClass
{
    public string Name { get; set; }
    public string Label { get; set; }
    public string[] Tags { get; set; }
    /// <summary>
    /// If form data is used for rendering a document, render data with dedicated template in template engine.
    /// </summary>
    public string TemplateName { get; set; }
}