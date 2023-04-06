using System.ComponentModel.DataAnnotations;

namespace IvNav.Store.Setup.Models;

/// <summary>
/// Paging request.
/// </summary>
public class PagingRequestDto
{
    /// <summary>
    /// Page.
    /// </summary>
    /// <example>1</example>
    [Range(1, int.MaxValue)]
    public int? Page { get; set; }

    /// <summary>
    /// PageSize.
    /// </summary>
    /// <example>25</example>
    [Range(5, 100)]
    public int? PageSize { get; set; }
}
