using System.ComponentModel.DataAnnotations;

namespace IvNav.Store.Web.Models.V1.Product;

/// <summary>
/// ReadProductsRequest.
/// </summary>
public class ReadProductsRequestDto
{
    /// <summary>
    /// Page.
    /// </summary>
    /// <example>1</example>
    [Range(1, int.MaxValue)]
    public int? Page { get; set; } = 1;

    /// <summary>
    /// PageSize.
    /// </summary>
    /// <example>25</example>
    [Range(5, 100)]
    public int? PageSize { get; set; } = 100;
}
