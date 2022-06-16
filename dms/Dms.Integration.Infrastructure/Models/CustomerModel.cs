using System.ComponentModel.DataAnnotations;

namespace Dms.Integration.Infrastructure.Models;

public class CustomerModel
{
    [MaxLength(36)]
    public string CustomerId { get; set; }
    [MaxLength(250)]
    public string FirstName { get; set; }
    [MaxLength(250)]
    public string LastName { get; set; }
    [MaxLength(11)]
    public long CitizenshipNumber { get; set; }
    public int CustomerNumber { get; set; }
}

