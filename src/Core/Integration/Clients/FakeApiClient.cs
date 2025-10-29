namespace Core.Integration.Clients;
public class FakeApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<FakeApiClient> _logger;

    public FakeApiClient(HttpClient httpClient, ILogger<FakeApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<WorkLogDto?> GetWorkLogAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/worklog/{id}", cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<WorkLogDto>(cancellationToken: cancellationToken);
        }
        catch (TaskCanceledException)
        {
            _logger.LogWarning("Request to get WorkLog {Id} timed out.", id);
            return default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling WorkLog endpoint for id {Id}", id);
            throw;
        }
    }
}