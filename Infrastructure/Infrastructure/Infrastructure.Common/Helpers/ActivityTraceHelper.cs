using System.Diagnostics;

namespace Infrastructure.Common.Helpers;

public static class ActivityTraceHelper
{
    public static void TrySetTraceId(Activity activity, string? traceId)
    {
        if (string.IsNullOrEmpty(traceId) || !IsValidTraceId(traceId))
            return;

        activity.SetParentId(
            ActivityTraceId.CreateFromString(traceId.AsSpan()),
            ActivitySpanId.CreateRandom(),
            ActivityTraceFlags.Recorded
        );
    }

    private static bool IsValidTraceId(string value) =>
        value.Length == 32 && value.All(IsHexDigit);

    private static bool IsHexDigit(char c) =>
        c is >= '0' and <= '9' or >= 'a' and <= 'f' or >= 'A' and <= 'F';
}