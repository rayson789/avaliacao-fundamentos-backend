namespace Core.Tests.Algorithms;
public class LargestIncreasingSequenceFinderTests
{
    [Fact]
    public void Deve_RetornarMaiorSequenciaCrescente()
    {
        // Arrange
        var finder = new LargestIncreasingSequenceFinder();
        var input = new[] { 1, 2, 3, 2, 3, 4, 5, 1 };

        // Act
        var result = finder.FindLongestIncreasingSequence(input);

        // Assert
        result.Should().BeEquivalentTo(new[] { 2, 3, 4, 5 }, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public void Deve_RetornarSequenciaVazia_QuandoListaVazia()
    {
        // Arrange
        var finder = new LargestIncreasingSequenceFinder();
        var input = Array.Empty<int>();

        // Act
        var result = finder.FindLongestIncreasingSequence(input);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Deve_RetornarPrimeiraSequencia_EmCasoDeEmpate()
    {
        // Arrange
        var finder = new LargestIncreasingSequenceFinder();
        var input = new[] { 1, 2, 3, 1, 2, 3 };

        // Act
        var result = finder.FindLongestIncreasingSequence(input);

        // Assert
        result.Should().BeEquivalentTo(new[] { 1, 2, 3 }, opt => opt.WithStrictOrdering());
    }
}
