namespace IvNav.Store.Setup.Models;

/// <summary>
/// IdDto
/// </summary>
public class IdDto<T>
{
    /// <summary>
    /// Id
    /// </summary>
    public T Id { get; init; } = default!;
}
