namespace Core.Tests.Integration;
public class FakeApiClientTests
{
    private class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _responseFunc;

        public FakeHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responseFunc)
        {
            _responseFunc = responseFunc;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_responseFunc(request));
        }
    }

    [Fact]
    public async Task Deve_Retornar_Dados_Em_Sucesso()
    {
        // Arrange
        var data = new WorkLogDto { Id = 1, Message = "OK", Status = "Done", Date = DateTime.UtcNow };
        var json = JsonSerializer.Serialize(data);
        var handler = new FakeHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json)
        });

        var client = new HttpClient(handler) { BaseAddress = new Uri("https://fakeapi.test/") };
        var apiClient = new FakeApiClient(client, NullLogger<FakeApiClient>.Instance);

        // Act
        var result = await apiClient.GetWorkLogAsync(1, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Status.Should().Be("Done");
    }

    [Fact]
    public async Task SDeve_Tratar_Timeout_De_Forma_Graciosa()
    {
        // Arrange
        var handler = new FakeHttpMessageHandler(_ => throw new TaskCanceledException());
        var client = new HttpClient(handler) { BaseAddress = new Uri("https://fakeapi.test/") };
        var apiClient = new FakeApiClient(client, NullLogger<FakeApiClient>.Instance);

        // Act
        var result = await apiClient.GetWorkLogAsync(99, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Deve_Lancar_Excecao_Em_Falha_Http()
    {
        // Arrange
        var handler = new FakeHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.InternalServerError));
        var client = new HttpClient(handler) { BaseAddress = new Uri("https://fakeapi.test/") };
        var apiClient = new FakeApiClient(client, NullLogger<FakeApiClient>.Instance);

        // Act
        var act = async () => await apiClient.GetWorkLogAsync(99, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }
}
