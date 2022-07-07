namespace Dms.Integration.Infrastructure.DocumentServices;

public class DocumentGroups
{
    public DocumentGroups()
    {
        TagList = new List<DocumentTag>();
    }
    public List<DocumentTag> TagList { get; set; }
}

public class DocumentTag
{
    public string TagId { get; set; }
    public string TagName { get; set; }
}