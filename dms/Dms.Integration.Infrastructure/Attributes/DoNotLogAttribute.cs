namespace Dms.Integration.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class DoNotLogAttribute : Attribute
{
}