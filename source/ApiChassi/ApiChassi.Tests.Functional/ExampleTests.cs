using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace ApiChassi.Tests.Functional;

public class ExampleTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ExampleTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task FetchShouldBeOk()
    {
        var response = await _client.GetAsync("/v1/example");
        response.EnsureSuccessStatusCode();
        Assert.Equal(200, (int)response.StatusCode);
    }
}
