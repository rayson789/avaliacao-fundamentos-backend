namespace Core.Concurrency;
public class ParallelProcessor
{
    /// <summary>
    /// Processa itens em paralelo e retorna a contagem por categoria (ex.: primeira letra).
    /// </summary>
    public async Task<Dictionary<string, int>> ProcessAsync(
        IEnumerable<string> items,
        int maxDegreeOfParallelism,
        CancellationToken cancellationToken = default,
        IProgress<int>? progress = null)
    {
        var result = new ConcurrentDictionary<string, int>();
        var total = items.Count();
        int processed = 0;

        await Parallel.ForEachAsync(
            items,
            new ParallelOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism,
                CancellationToken = cancellationToken
            },
            async (item, token) =>
            {
                token.ThrowIfCancellationRequested();

                // Simula processamento (ex: I/O, API, etc.)
                await Task.Delay(1, token);

                var category = item.Substring(0, 1).ToUpperInvariant();
                result.AddOrUpdate(category, 1, (_, count) => count + 1);

                // Atualiza progresso
                var current = Interlocked.Increment(ref processed);
                progress?.Report((current * 100) / total);
            });

        return result.ToDictionary(kv => kv.Key, kv => kv.Value);
    }
}

