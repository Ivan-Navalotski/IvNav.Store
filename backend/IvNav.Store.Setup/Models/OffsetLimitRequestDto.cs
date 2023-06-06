using System.ComponentModel.DataAnnotations;

namespace IvNav.Store.Setup.Models;

/// <summary>
/// Paging request.
/// </summary>
public class OffsetLimitRequestDto
{
    /// <summary>
    /// Offset.
    /// </summary>
    /// <example>0</example>
    [Range(0, int.MaxValue)]
    public int? Offset { get; set; }

    /// <summary>
    /// Limit.
    /// </summary>
    /// <example>25</example>
    [Range(1, 100)]
    public int? Limit { get; set; }
}
