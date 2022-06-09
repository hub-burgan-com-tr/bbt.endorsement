using Worker.App.Application.Internals.Models;

namespace Worker.App.Models;

public static class Users
{
    public static List<GsmPhones> GsmPhones()
    {
        var list = new List<GsmPhones>
        {
            new GsmPhones { County = 90, Prefix = 0, Number = 0 },
            new GsmPhones { County = 90, Prefix = 0, Number = 0 },
            new GsmPhones { County = 90, Prefix = 0, Number = 0 },
            new GsmPhones { County = 90, Prefix = 0, Number = 0 },
            new GsmPhones { County = 90, Prefix = 0, Number = 0 },
            new GsmPhones { County = 90, Prefix = 0, Number = 0 },
            new GsmPhones { County = 90, Prefix = 0, Number = 0 }
        };

        return list;
    }
    public static List<string> Emails()
    {
        var list = new List<string>
        {
            ""
        };
        return list;
    }
}

