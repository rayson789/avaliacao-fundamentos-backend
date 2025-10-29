namespace Core.Design.Implementations;
public class ExternalServiceSimulator : IExternalService
{
    private readonly string _serviceName;
    private readonly ILogger<ExternalServiceSimulator> _logger;
    private readonly Random _random = new();

    public ExternalServiceSimulator(string serviceName, ILogger<ExternalServiceSimulator> logger)
    {
        _serviceName = serviceName;
        _logger = logger;
    }

    public async Task<string> GetDataAsync(CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(_random.Next(100, 300), cancellationToken);

            // Simula falha com 30% de chance
            if (_random.NextDouble() < 0.3)
                throw new Exception($"Simulated failure on service {_serviceName}");

            return $"Success: {_serviceName}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling service {ServiceName}", _serviceName);
            throw;
        }
    }
}
