using System.ComponentModel.DataAnnotations;

namespace IvNav.Store.Web.Models.V1.Product;

/// <summary>
/// CreateProductDto
/// </summary>
public class CreateProductRequestDto
{
    /// <summary>
    /// Name
    /// </summary>
    /// <example>Product1</example>
    [Required]
    public string? Name { get; set; }
}
