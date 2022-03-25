namespace Worker.App.Models;


public class OrderApprover
{
    public long ClientNumber { get; set; }
    public long CitizenshipNumber { get; set; }
    public NameClass Name { get; set; }
    public GsmPhone[] GsmPhones { get; set; }
    public string[] Emails { get; set; }
    public Device[] Devices { get; set; }

    public class NameClass
    {
        public string First { get; set; }
        public string Last { get; set; }
    }
    public class Device
    {
        public string DeviceId { get; set; }
        public string Label { get; set; }
    }
    public class GsmPhone
    {
        public int County { get; set; }
        public long Prefix { get; set; }
        public long Number { get; set; }
    }

}
