namespace Core.Algorithms;
public class LargestIncreasingSequenceFinder
{
    /// <summary>
    /// Retorna a maior sequência crescente consecutiva.
    /// Em caso de empate, retorna a sequência que começa antes na lista.
    /// </summary>
    public List<int> FindLongestIncreasingSequence(IList<int> numbers)
    {
        if (numbers == null || numbers.Count == 0)
            return new List<int>();

        var best = new List<int>();
        var current = new List<int> { numbers[0] };

        for (int i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] > numbers[i - 1])
            {
                current.Add(numbers[i]);
            }
            else
            {
                if (current.Count > best.Count)
                    best = new List<int>(current);
                current = new List<int> { numbers[i] };
            }
        }

        if (current.Count > best.Count)
            best = current;

        return best;
    }
}