using System.ComponentModel.DataAnnotations;

namespace IvNav.Store.Web.Models.V1.Product;

/// <summary>
/// Create product.
/// </summary>
public sealed class CreateProductRequestDto
{
    /// <summary>
    /// Product name.
    /// </summary>
    /// <example>Product1</example>
    [Required]
    public string? Name { get; set; }
}
