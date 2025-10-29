namespace Core.Design.Orchestration;
public class AggregatorService
{
    private readonly IEnumerable<IExternalService> _services;
    private readonly ILogger<AggregatorService> _logger;
    private readonly MetricsRecorder _metrics;

    public AggregatorService(
        IEnumerable<IExternalService> services,
        ILogger<AggregatorService> logger)
    {
        _services = services;
        _logger = logger;
        _metrics = new MetricsRecorder();
    }

    public async Task<(List<string> Successes, List<string> Errors)> AggregateAsync(CancellationToken cancellationToken)
    {
        using (_logger.BeginOperationScope("AggregateAsync"))
        {
            var successes = new List<string>();
            var errors = new List<string>();

            _logger.LogInformation("Starting aggregation for {ServiceCount} services...", _services.Count());

            foreach (var service in _services)
            {
                using var op = _metrics.StartOperation();

                try
                {
                    var result = await service.GetDataAsync(cancellationToken);
                    successes.Add(result);

                    _logger.LogInformation(
                        "Service {Service} succeeded with result '{Result}'",
                        service.GetType().Name,
                        result
                    );
                }
                catch (Exception ex)
                {
                    op.MarkFailed();

                    _logger.LogWarning(
                        ex,
                        "Service {Service} failed. Continuing with remaining services.",
                        service.GetType().Name
                    );

                    errors.Add(ex.Message);
                }
            }

            _logger.LogInformation(
                "Aggregation completed. AverageTime={AverageMs}ms | ErrorRate={ErrorRate:P2}",
                _metrics.GetAverageDurationMs(),
                _metrics.GetErrorRate()
            );

            return (successes, errors);
        }
    }
}