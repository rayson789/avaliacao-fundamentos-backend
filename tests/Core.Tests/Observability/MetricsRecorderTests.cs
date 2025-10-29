namespace Core.Tests.Observability;
public class MetricsRecorderTests
{
    [Fact]
    public void Deve_Calcular_Tempo_Medio_E_Taxa_De_Erro_Corretamente()
    {
        // Arrange
        var metrics = new MetricsRecorder();

        // Act
        for (int i = 0; i < 5; i++)
        {
            using var op = metrics.StartOperation();

            if (i % 2 == 0)
                op.MarkFailed();

            Thread.Sleep(10);
        }

        // Assert
        metrics.GetAverageDurationMs().Should().BeGreaterThan(0);
        metrics.GetErrorRate().Should().BeApproximately(0.6, 0.1);
    }

    [Fact]
    public void Deve_Retornar_Zero_Para_Metricas_Sem_Operacoes()
    {
        // Arrange
        var metrics = new MetricsRecorder();

        // Assert
        metrics.GetAverageDurationMs().Should().Be(0);
        metrics.GetErrorRate().Should().Be(0);
    }
}
