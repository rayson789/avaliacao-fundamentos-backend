namespace Core.Tests.Design.Orchestration;
public class AggregatorServiceObservabilityTests
{
    private readonly Mock<ILogger<AggregatorService>> _mockLogger = new();

    private class SuccessService : IExternalService
    {
        public Task<string> GetDataAsync(CancellationToken cancellationToken)
            => Task.FromResult("OK");
    }

    private class FailService : IExternalService
    {
        public Task<string> GetDataAsync(CancellationToken cancellationToken)
            => throw new InvalidOperationException("Erro simulado");
    }

    [Fact]
    public async Task Deve_Gerar_Logs_E_Metricas_Com_Sucesso_E_Falha()
    {
        // Arrange
        var services = new List<IExternalService> { new SuccessService(), new FailService() };
        var aggregator = new AggregatorService(services, _mockLogger.Object);

        // Act
        var (successes, errors) = await aggregator.AggregateAsync(CancellationToken.None);

        // Assert
        successes.Should().HaveCount(1);
        errors.Should().HaveCount(1);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.AtLeastOnce);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}