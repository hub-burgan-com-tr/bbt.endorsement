using Dms.Integration.Infrastructure.Attributes;

namespace Dms.Integration.Infrastructure.Extensions;

public static class EnumExtensions
{
    public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
    {
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        if (memInfo == null || memInfo.Length == 0)
        {
            return null;
        }
        var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
        return (attributes.Length > 0) ? (T)attributes[0] : null;
    }

    public static string GetAttributeDescription(this Enum enumValue)
    {
        if (enumValue == null)
        {
            return string.Empty;
        }
        var attribute = enumValue.GetAttributeOfType<NameAttribute>();
        return attribute == null ? String.Empty : attribute.name;
    }
}