using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace IvNav.Store.Setup.Middleware;

/// <summary>
/// ActivityMiddleware
/// </summary>
public class ActivityMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _applicationName;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="next"></param>
    /// <param name="applicationName"></param>
    public ActivityMiddleware(
        RequestDelegate next, string applicationName)
    {
        _next = next;
        _applicationName = applicationName;
    }

    /// <summary>
    /// Invoke
    /// </summary>
    /// <param name="context"></param>
    public async Task Invoke(HttpContext context)
    {
        Activity.Current ??= new Activity(_applicationName);
        Init(Activity.Current, _applicationName);
        await _next(context);
    }

    private static void Init(Activity? activity, string applicationName)
    {
        if (activity == null) return;

        activity.DisplayName = applicationName;


        if (!string.IsNullOrEmpty(activity.TraceStateString))
        {
            var statuses =
                activity.TraceStateString.Split(",")
                    .Where(i => !string.IsNullOrEmpty(i))
                    .Select(i =>
                    {
                        var values = i.Split("=");
                        return values.Length != 2
                            ? new KeyValuePair<string, string>("", "")
                            : new KeyValuePair<string, string>(values[0], values[1]);
                    })
                    .Where(i => !string.IsNullOrEmpty(i.Key))
                    .Reverse()
                    .ToArray();

            foreach (var (key, value) in statuses)
            {
                //activity.SetBaggage(key, value);
                activity.AddBaggage(key, value);
            }
        }
    }
}
