using System.Diagnostics;

namespace IvNav.Store.Common.Extensions;

public static class ActivityExtensions
{
    public static Dictionary<string, string> GetRequestHeaders(this Activity? activity)
    {
        var res = new Dictionary<string, string>();
        if (activity == null) return res;

        var traceId = activity.Id;
        if (!string.IsNullOrEmpty(traceId))
        {
            res.Add("traceparent", traceId);

            var stack = new List<KeyValuePair<string, string>> { new(activity.DisplayName, activity.SpanId.ToHexString()) };
            stack.AddRange(activity.Baggage!);

            var tracestate = string.Join(",",
                stack.Select(i => $"{i.Key}={i.Value}")
            );

            res.Add("tracestate", tracestate);
        }

        return res;
    }
}
