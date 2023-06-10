namespace IvNav.Store.Common.Models.Abstractions.Paging;

public interface IOffsetLimitRequest
{
    public int? Offset { get; }

    public int? Limit { get; }
}
