using IvNav.Store.Core.Models.Abstractions.Paging;

namespace IvNav.Store.Core.Extensions.Common;

internal static class QueryableExtensions
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, IPagingRequest? filter)
    {
        if (filter == null)
        {
            return query;
        }

        if (filter.PageSize == null || filter.Page == null)
        {
            return query;
        }

        var skip = (filter.Page.Value > 0 ? filter.Page.Value - 1 : 0) * filter.PageSize.Value;

        var queryNew = query.Skip(skip).Take(filter.PageSize.Value);
        return queryNew;
    }
}
