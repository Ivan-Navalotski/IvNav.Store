namespace IvNav.Store.Core.Interaction.Attributes;

internal class AppIdAttribute : Attribute
{
    public AppIdAttribute(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the value stored in this attribute.
    /// </summary>
    public virtual string Value { get; }
}
