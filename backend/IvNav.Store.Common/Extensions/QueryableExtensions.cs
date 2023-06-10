using IvNav.Store.Common.Models.Abstractions.Paging;

namespace IvNav.Store.Common.Extensions;

public static class QueryableExtensions
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
