using System.Text.Json;
using RepoRanger.Application.Contracts.VersionControlSystems;

namespace RepoRanger.Api.IntegrationTests.Controllers;

public class VersionControlSystemsControllerTests : TestBase
{
    [Test]
    public async Task List_ReturnsSources()
    {
        // Act
        var response = await Client.GetAsync("/api/VersionControlSystems");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var versionControlSystems
            = JsonSerializer.Deserialize<VersionControlSystemsVm>(responseString);

        Assert.That(versionControlSystems, Is.Not.Null);
        Assert.That(versionControlSystems.VersionControlSystems, Is.Not.Empty);
        Assert.That(versionControlSystems.VersionControlSystems, Has.Count.EqualTo(2));
    }
}