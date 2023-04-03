namespace IvNav.Store.Core.Models.Abstractions.Paging;

public interface IPagingRequest
{
    public int? Page { get; }

    public int? PageSize { get; }
}
