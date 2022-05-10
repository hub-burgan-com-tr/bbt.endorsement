using Dms.Integration.Infrastructure.DocumentServices;
using System.ComponentModel.DataAnnotations;

namespace Dms.Integration.Infrastructure.Models;

public class DocumentModel : DocumentBase
{
    [MaxLength(36)]
    public string DocumentId { get; set; }
    [MaxLength(36)]
    public string OrderId { get; set; }
    [MaxLength(36)]
    public string FormDefinitionId { get; set; }
    [MaxLength(250)]
    public string Name { get; set; }
    public string Content { get; set; }
    [MaxLength(50)]
    public string Type { get; set; }
    [MaxLength(50)]
    public string FileType { get; set; }
    [MaxLength(50)]
    public string State { get; set; }
    [MaxLength(250)]
    public string MimeType { get; set; }

    /// <summary>
    /// belge fiziksel olarak bankaya ulaştı mı ?
    /// </summary>
    public bool? IsPhysicalFileDelivered { get; set; }

    /// <summary>
    /// Belge fiziksel olarak ulaştığına dair note.
    /// </summary>
    public string PhysicalFileDeliverNote { get; set; }

    public PersonModel Owner { get; set; }
}

