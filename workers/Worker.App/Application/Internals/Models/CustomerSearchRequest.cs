namespace Worker.App.Application.Internals.Models;


public class CustomerSearchRequest
{
    public string name { get; set; }
    public int page { get; set; } = 1;
    public int size { get; set; } = 10;
}
