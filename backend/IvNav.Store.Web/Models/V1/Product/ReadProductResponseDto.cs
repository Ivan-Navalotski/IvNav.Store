namespace IvNav.Store.Web.Models.V1.Product;

/// <summary>
/// ProductDto
/// </summary>
public class ReadProductResponseDto
{
    /// <summary>
    /// Id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name.
    /// </summary>
    public string Name { get; set; } = null!;
}
