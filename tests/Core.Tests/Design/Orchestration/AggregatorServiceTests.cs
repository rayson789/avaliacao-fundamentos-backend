namespace Core.Tests.Design.Orchestration;
public class AggregatorServiceTests
{
    private class StubService : IExternalService
    {
        private readonly string _name;
        private readonly bool _shouldFail;

        public StubService(string name, bool shouldFail = false)
        {
            _name = name;
            _shouldFail = shouldFail;
        }

        public Task<string> GetDataAsync(CancellationToken cancellationToken)
        {
            if (_shouldFail)
                throw new Exception($"Simulated failure in {_name}");

            return Task.FromResult($"OK: {_name}");
        }
    }

    [Fact]
    public async Task Should_Return_All_Results_When_No_Errors()
    {
        // Arrange
        var services = new List<IExternalService>
        {
            new StubService("S1"),
            new StubService("S2"),
            new StubService("S3")
        };

        var aggregator = new AggregatorService(services, NullLogger<AggregatorService>.Instance);

        // Act
        var (successes, errors) = await aggregator.AggregateAsync(CancellationToken.None);

        // Assert
        successes.Should().HaveCount(3);
        errors.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_Aggregate_Partial_Success_When_One_Service_Fails()
    {
        // Arrange
        var services = new List<IExternalService>
        {
            new StubService("S1"),
            new StubService("S2", shouldFail: true),
            new StubService("S3")
        };

        var aggregator = new AggregatorService(services, NullLogger<AggregatorService>.Instance);

        // Act
        var (successes, errors) = await aggregator.AggregateAsync(CancellationToken.None);

        // Assert
        successes.Should().HaveCount(2);
        errors.Should().ContainSingle().And.Contain(e => e.Contains("S2"));
    }

    [Fact]
    public async Task Should_Return_Only_Errors_When_All_Services_Fail()
    {
        // Arrange
        var services = new List<IExternalService>
        {
            new StubService("S1", true),
            new StubService("S2", true),
            new StubService("S3", true)
        };

        var aggregator = new AggregatorService(services, NullLogger<AggregatorService>.Instance);

        // Act
        var (successes, errors) = await aggregator.AggregateAsync(CancellationToken.None);

        // Assert
        successes.Should().BeEmpty();
        errors.Should().HaveCount(3);
    }
}