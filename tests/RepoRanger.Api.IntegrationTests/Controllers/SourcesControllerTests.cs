using System.Text.Json;
using RepoRanger.Application.Contracts.Sources;

namespace RepoRanger.Api.IntegrationTests.Controllers;

public class SourcesControllerTests : TestBase
{
    [Test]
    public async Task List_ReturnsSources()
    {
        // Act
        var response = await Client.GetAsync("/api/sources");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var sources = JsonSerializer.Deserialize<SourcesVm>(responseString);

        Assert.That(sources, Is.Not.Null);
        Assert.That(sources.Sources, Is.Not.Empty);
        Assert.That(sources.Sources.Count, Is.EqualTo(2));
    }
}