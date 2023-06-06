namespace IvNav.Store.Core.Models.Abstractions.Paging;

public interface IOffsetLimitRequest
{
    public int? Offset { get; }

    public int? Limit { get; }
}
