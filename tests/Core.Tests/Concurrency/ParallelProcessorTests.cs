namespace Core.Tests.Concurrency;
public class ParallelProcessorTests
{
    [Fact]
    public async Task Deve_processar_itens_em_paralelo_e_agrupar_por_categoria()
    {
        // Arrange
        var processor = new ParallelProcessor();
        var items = Enumerable.Range(1, 10000).Select(i => i % 2 == 0 ? "even" : "odd");
        var token = CancellationToken.None;

        // Act
        var result = await processor.ProcessAsync(items, maxDegreeOfParallelism: 8, token);

        // Assert
        result.Should().ContainKey("E").And.ContainKey("O");
        result["E"].Should().Be(5000);
        result["O"].Should().Be(5000);
    }

    [Fact]
    public async Task Deve_cancelar_processamento_ordenadamente()
    {
        // Arrange
        var processor = new ParallelProcessor();
        var items = Enumerable.Range(1, 10000).Select(i => i.ToString());
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(10);

        // Act
        Func<Task> act = async () => await processor.ProcessAsync(items, 4, cts.Token);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }
}
