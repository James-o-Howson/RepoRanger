using Microsoft.Extensions.DependencyInjection;

namespace RepoRanger.Api.IntegrationTests;

public class TestBase : IDisposable
{
    protected IntegrationTestWebApplicationFactory Factory;
    protected HttpClient Client;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        Factory = new IntegrationTestWebApplicationFactory();
    }

    [SetUp]
    public void Setup() => Client = Factory.CreateClient();

    public void Dispose() => Factory.Dispose();

    public TService GetRequiredService<TService>() where TService : notnull
        => Factory.Services.GetRequiredService<TService>();
}