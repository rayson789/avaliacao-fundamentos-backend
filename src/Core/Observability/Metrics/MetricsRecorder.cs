namespace Core.Observability.Metrics;
public class MetricsRecorder
{
    private readonly List<TimeSpan> _durations = new();
    private int _errorCount = 0;
    private int _totalCount = 0;

    public MetricScope StartOperation()
    {
        var stopwatch = Stopwatch.StartNew();
        _totalCount++;
        return new MetricScope(stopwatch, this);
    }

    internal void Record(TimeSpan duration, bool success)
    {
        _durations.Add(duration);
        if (!success) _errorCount++;
    }

    public double GetAverageDurationMs()
        => _durations.Count == 0 ? 0 : _durations.Average(d => d.TotalMilliseconds);

    public double GetErrorRate()
        => _totalCount == 0 ? 0 : (double)_errorCount / _totalCount;

    public sealed class MetricScope : IDisposable
    {
        private readonly Stopwatch _stopwatch;
        private readonly MetricsRecorder _recorder;
        private bool _success = true;

        public MetricScope(Stopwatch stopwatch, MetricsRecorder recorder)
        {
            _stopwatch = stopwatch;
            _recorder = recorder;
        }

        public void MarkFailed() => _success = false;

        public void Dispose()
        {
            _stopwatch.Stop();
            _recorder.Record(_stopwatch.Elapsed, _success);
        }
    }
}
