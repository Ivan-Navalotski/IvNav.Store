using System.Diagnostics.Contracts;

namespace IvNav.Store.Common.Extensions;

public static class ExceptionExtensions
{
    [Pure]
    public static IEnumerable<string> FlattenHierarchy(this Exception? ex)
    {
        if (ex != null)
        {
            yield return ex.Message;

            if (ex is AggregateException aggregateException)
            {
                foreach (var inner in aggregateException.InnerExceptions)
                {
                    yield return inner.Message;
                }
            }
            else if (ex.InnerException is { } inner)
            {
                yield return inner.Message;
            }
        }
    }
}
