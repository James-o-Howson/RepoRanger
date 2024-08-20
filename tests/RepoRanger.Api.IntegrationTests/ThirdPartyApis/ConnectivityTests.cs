using ThirdPartyClients.Generated;

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
        var query = new V1Query
        {
            Version = "4.1.2",
            Package = new OsvPackage
            {
                Name = "IdentityServer4",
                Ecosystem = "NuGet"
            }
        };

        var response = await _vulnerabilitiesClient.QueryAffectedAsync(query);
        
        Assert.That(response, Is.Not.Null);
        Assert.That(response, Is.TypeOf<V1VulnerabilityList>());
    }
}