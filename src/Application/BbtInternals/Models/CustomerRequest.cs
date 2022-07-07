﻿namespace Application.BbtInternals.Models;

public class CustomerRequest
{
    //public string customerName { get; set; }
    public CustomerName name { get; set; }
    public string identityNumber { get; set; }
    public int customerNumber { get; set; }
    public int page { get; set; } = 1;
    public int size { get; set; } = 10;
}

public class CustomerName
{
    public string first { get; set; }
    public string last { get; set; }
}

public class CustomerSearchRequest
{
    public string name { get; set; }
    public int page { get; set; } = 1;
    public int size { get; set; } = 10;
}
