namespace Core.Tests.Algorithms;
public class AlgorithmsTests
{
    [Fact]
    public void Deve_RetornarPrimeiroNumeroRepetido()
    {
        // Arrange
        var finder = new RepeatedNumberFinder();
        var input = new[] { 1, 2, 3, 2, 5 };

        // Act
        var result = finder.FindFirstRepeated(input);

        // Assert
        result.Should().Be(2);
    }

    [Fact]
    public void Deve_RetornarNull_QuandoNaoHaRepeticao()
    {
        // Arrange
        var finder = new RepeatedNumberFinder();
        var input = new[] { 1, 2, 3, 4 };

        // Act
        var result = finder.FindFirstRepeated(input);

        // Assert
        result.Should().BeNull();
    }
}
