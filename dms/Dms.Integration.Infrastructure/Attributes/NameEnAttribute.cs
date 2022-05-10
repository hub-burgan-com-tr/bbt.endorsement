namespace Dms.Integration.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class NameEnAttribute : Attribute
{
    public readonly string name;

    public NameEnAttribute(string name)
    {
        this.name = name;
    }
}