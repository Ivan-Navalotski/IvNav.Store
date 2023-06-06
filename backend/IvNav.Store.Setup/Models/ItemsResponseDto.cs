namespace IvNav.Store.Setup.Models;

/// <summary>
/// Items response.
/// </summary>
/// <typeparam name="TItem"></typeparam>
public class ItemsResponseDto<TItem>
{
    /// <summary>
    /// Items.
    /// </summary>
    public IReadOnlyCollection<TItem> Items { get; init; } = new List<TItem>(0);


    public int TotalCount { get; init; }
}
