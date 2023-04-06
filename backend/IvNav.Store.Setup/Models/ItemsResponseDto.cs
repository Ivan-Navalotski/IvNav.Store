using Ardalis.GuardClauses;

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
    public IReadOnlyCollection<TItem> Items { get; init; }

    /// <summary>
    /// Ctor.
    /// </summary>
    public ItemsResponseDto()
        : this(Array.Empty<TItem>())
    {
    }

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="entities"></param>
    public ItemsResponseDto(IReadOnlyCollection<TItem> entities)
    {
        Items = Guard.Against.Null(entities);
    }
}
