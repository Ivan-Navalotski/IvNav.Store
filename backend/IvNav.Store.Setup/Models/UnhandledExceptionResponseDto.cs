namespace IvNav.Store.Setup.Models;

/// <summary>
/// UnhandledExceptionResponseDto
/// </summary>
public class UnhandledExceptionResponseDto
{
    /// <summary>
    /// Message
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// TraceId
    /// </summary>
    public string TraceId { get; init; } = null!;
}
