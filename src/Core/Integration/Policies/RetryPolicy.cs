namespace Core.Integration.Policies;
public static class RetryPolicy
{
    public static IAsyncPolicy<HttpResponseMessage> GetExponentialRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt))
            );
    }
}