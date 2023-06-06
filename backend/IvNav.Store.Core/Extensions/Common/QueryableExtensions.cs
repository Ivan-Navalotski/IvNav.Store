using IvNav.Store.Core.Models.Abstractions.Paging;

namespace IvNav.Store.Core.Extensions.Common;

internal static class QueryableExtensions
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, IOffsetLimitRequest? request)
    {
        if (request == null)
        {
            return query;
        }

        if (request.Offset == null || request.Limit == null)
        {
            return query;
        }

        var queryNew = query.Skip(request.Offset.Value).Take(request.Limit.Value);
        return queryNew;
    }
}
