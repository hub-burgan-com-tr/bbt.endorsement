namespace Dms.Integration.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class NameAttribute : Attribute
{
    public readonly string name;

    public NameAttribute(string name)
    {
        this.name = name;
    }
}