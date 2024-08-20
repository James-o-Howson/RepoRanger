using RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities;
using RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Request;
using RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Response;
using RepoRanger.Infrastructure.ThirdPartyClients.OpenSourceVulnerabilities.Schema;

namespace RepoRanger.Api.IntegrationTests.ThirdPartyApis;

public class ConnectivityTests : TestBase
{
    private IOsvClient _vulnerabilitiesClient;

    [SetUp]
    public void SetUp()
    {
        _vulnerabilitiesClient = GetRequiredService<IOsvClient>();
    }

    [Test]
    public async Task OsvVulnerabilitiesApi_ShouldBeReachable()
    {
        var query = new Query
        {
            Version = "4.1.2",
            Package = new Package
            {
                Name = "IdentityServer4",
                Ecosystem = Ecosystem.NuGet
            }
        };

        var response = await _vulnerabilitiesClient.QueryAffectedAsync(query);
        
        Assert.That(response, Is.Not.Null);
        Assert.That(response, Is.TypeOf<VulnerabilityList>());
    }
}