namespace IvNav.Store.Web.Models.V1.Product;

/// <summary>
/// GetProductResponse
/// </summary>
public class GetProductResponse
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; } = null!;
}
