namespace Core.Algorithms;
public class RepeatedNumberFinder
{
    /// <summary>
    /// Retorna o primeiro número repetido na sequência, ou null se não houver repetição.
    /// </summary>
    public int? FindFirstRepeated(IEnumerable<int> numbers)
    {
        var seen = new HashSet<int>();

        foreach (var n in numbers)
        {
            if (!seen.Add(n))
                return n;
        }

        return null;
    }
}
