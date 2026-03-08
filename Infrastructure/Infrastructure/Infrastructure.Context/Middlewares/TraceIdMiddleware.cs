using System.Diagnostics;
using Infrastructure.Common.Helpers;
using Infrastructure.Context.Constants;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Context.Middlewares;

public class TraceIdMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        using var activity = new Activity(TraceConstants.TraceIdHeaderName);
        activity.SetIdFormat(ActivityIdFormat.W3C);

        ActivityTraceHelper.TrySetTraceId(
            activity,
            context.Request.Headers[TraceConstants.TraceIdHeaderName].FirstOrDefault()
        );

        activity.Start();
        var traceId = activity.TraceId.ToString();

        context.Response.OnStarting(() =>
        {
            context.Response.Headers[TraceConstants.TraceIdHeaderName] = traceId;
            return Task.CompletedTask;
        });

        await next(context);
    }
}
