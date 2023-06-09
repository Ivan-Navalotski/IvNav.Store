namespace IvNav.Store.Web.Models.V1.Product;

/// <summary>
/// Product.
/// </summary>
public sealed class ReadProductResponseDto
{
    /// <summary>
    /// Product identifier.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Product name.
    /// </summary>
    public string Name { get; init; } = null!;
}
