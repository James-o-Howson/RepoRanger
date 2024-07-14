using ThirdPartyApiClient;

namespace RepoRanger.Api.IntegrationTests.ThirdPartyApis;

[TestFixture]
public class ConnectivityTests : TestBase
{
    private IOpenSourceVulnerabilitiesClient _vulnerabilitiesClient;

    [SetUp]
    public void SetUp()
    {
        _vulnerabilitiesClient = GetRequiredService<IOpenSourceVulnerabilitiesClient>();
    }

    [Test]
    public async Task CanReachOpenSourceVulnerabilitiesApi()
    {
        var v1Query = new V1Query
        {
            Package = new OsvPackage
            {
                Name = "MediatR",
                Ecosystem = "NuGet"
            }
        };
        
        var response = await _vulnerabilitiesClient.QueryAffectedAsync(v1Query);
        
        Assert.That(response, Is.Not.Null);
        Assert.That(response, Is.TypeOf<V1VulnerabilityList>());
    }
}