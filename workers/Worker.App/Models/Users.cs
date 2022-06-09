using Worker.App.Application.Internals.Models;

namespace Worker.App.Models;

public static class Users
{
    public static List<GsmPhones> GsmPhones()
    {
        var list = new List<GsmPhones>
        {
            new GsmPhones { County = 90, Prefix = 542, Number = 4729390 },
            new GsmPhones { County = 90, Prefix = 554, Number = 3396048 },
            new GsmPhones { County = 90, Prefix = 533, Number = 4571673 },
            new GsmPhones { County = 90, Prefix = 530, Number = 5695988 },
            new GsmPhones { County = 90, Prefix = 505, Number = 3121304 },
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
            "htoremen@burgan.com.tr",
            "KKarakus@burgan.com.tr",
            "VSerapYilankirkan@burgan.com.tr",
            "LYaktubay@burgan.com.tr",
            "FKalender@burgan.com.tr",
            "",
            "",
            ""
        };
        return list;
    }
}

