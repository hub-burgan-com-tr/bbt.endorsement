using Dms.Integration.Infrastructure.Document;
using Dms.Integration.Infrastructure.Attributes;

namespace Dms.Integration.Infrastructure.DocumentServices;

public abstract class DocumentBase
{
    public DocumentDefinition Definition { get; set; }
    public List<DocumentContent> Contents { get; set; }
    public DocumentActionType? OwnerActionType { get; set; }
}

public enum DocumentActionType
{
    /// <summary>
    /// Hic birsey yapilmadi
    /// </summary>
    [Name("Hic birsey yapılmadı")]
    None = 10,

    /// <summary>
    /// Okudum/anladim
    /// </summary>
    [Name("Online İmzalı")]
    OnlineSigned = 20,

    /// <summary>
    /// Islak imzali
    /// </summary>
    [Name("Islak imzalı")]
    PhysicallySigned = 30,

    /// <summary>
    /// Belge/bilgi goruldu onaylandi
    /// </summary>
    [Name("Onaylandı")]
    Checked = 40,

    /// <summary>
    /// Belge/bilgi goruldu onaylanmadi
    /// </summary>
    [Name("Onaylanmadı")]
    NotChecked = 50,

    /// <summary>
    /// Belge/bilgi tarandi veya fotokopilendi. Kuryeden gelir.
    /// </summary>
    [Name("Kurye")]
    Copied = 60,

    /// <summary>
    /// Musteriye sesli okundu.
    /// </summary>
    [Name("Müşteriye sesli okundu")]
    ReadToCustomer = 70,

    /// <summary>
    /// Musteriye gönderildi.
    /// </summary>
    [Name("Müşteriye gönderildi")]
    SendToCustomer = 80
}