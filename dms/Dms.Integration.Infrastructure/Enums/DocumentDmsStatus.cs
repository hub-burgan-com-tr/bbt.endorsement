namespace Dms.Integration.Infrastructure.Enums;

public enum DocumentDmsStatus
{
    /// <summary>
    /// Ok (created/updated)
    /// </summary>
    Ok = 10,

    /// <summary>
    /// Could not create in DMS
    /// </summary>
    CouldNotCreate = 20,

    /// <summary>
    /// Could not update in DMS
    /// </summary>
    CouldNotUpdate = 30,
}