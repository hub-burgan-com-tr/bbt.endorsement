using System.ComponentModel.DataAnnotations;

namespace Dms.Integration.Infrastructure.Models;

public class DmsPerson
{
    [MaxLength(250)]
    public string FirstName { get; set; }
    [MaxLength(250)]
    public string LastName { get; set; }
    [MaxLength(11)]
    public long CitizenshipNumber { get; set; }
    public UInt64 ClientNumber { get; set; }
}

