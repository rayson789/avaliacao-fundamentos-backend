namespace Core.Observability.Logging;
public static class LogContext
{
    public static IDisposable BeginOperationScope(this ILogger logger, string operationName)
    {
        var operationId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

        return logger.BeginScope(new Dictionary<string, object>
        {
            ["operationId"] = operationId,
            ["operationName"] = operationName,
            ["timestamp"] = DateTime.UtcNow
        }) ?? NullScope.Instance;
    }

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();
        public void Dispose() { }
    }
}