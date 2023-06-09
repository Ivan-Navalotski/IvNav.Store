using Ardalis.GuardClauses;

namespace IvNav.Store.Setup.Models;

public class RequestHeaderData
{
    public string HeaderName { get; }
    public string? Description { get; }
    public string? DefaultValue { get; }
    public bool IsRequired { get; }
    public Type? Type { get; }

    public RequestHeaderData(string headerName, string? description = null, string? defaultValue = null, bool isRequired = false, Type? type = null)
    {
        HeaderName = Guard.Against.NullOrEmpty(headerName);
        Description = description;
        DefaultValue = defaultValue;
        IsRequired = isRequired;
        Type = type != null ? Guard.Against.InvalidInput(type, nameof(type), i => i.IsValueType) : null;
    }
}
