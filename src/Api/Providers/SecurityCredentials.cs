namespace Api.Providers;

public class SecurityCredential
{
    public string Key { get; internal set; }
    public string Value { get; internal set; }

    public SecurityCredential(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public override string ToString()
    {
        return $"{Key}{SecurityCredentials.CredentialSeperator}{Value}";
    }

    public static SecurityCredential Parse(string content)
    {
        var exceptionMessage = $"the value that \"{content}\" of \"{nameof(content)}\" parameter can not parse";

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new InvalidOperationException(exceptionMessage);
        }

        var values = content.Split(SecurityCredentials.CredentialSeperator);

        if (values.Length != 2)
        {
            throw new InvalidOperationException(exceptionMessage);
        }

        return new SecurityCredential(values[0], values[1]);
    }

    public static bool TryParse(string content, out SecurityCredential credential)
    {
        credential = null;

        try
        {
            credential = Parse(content);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}


/// <summary>
/// SecurityCredentials
/// </summary>
public static class SecurityCredentials
{
    public const string CredentialSeperator = "###";

    public readonly static IReadOnlyList<SecurityCredential> DefaultRegisterActionRequirements = new List<SecurityCredential>
                            {  };


    public readonly static SecurityCredential IsDefault = new SecurityCredential("IsDefault", "1");

    public static IDictionary<string, string> ToDictionary(this IReadOnlyList<SecurityCredential> securityCredentialList)
    {
        return securityCredentialList.Select(t => new { t.Key, t.Value })
              .ToDictionary(t => t.Key, t => t.Value);
    }

    public static string AsString(this IReadOnlyList<SecurityCredential> securityCredentialList)
    {
        return securityCredentialList.Select(t => t.ToString())
               .Aggregate((a, b) => a + ";" + b);
    }
}

