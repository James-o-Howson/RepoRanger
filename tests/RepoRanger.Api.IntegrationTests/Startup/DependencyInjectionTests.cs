using Microsoft.Extensions.DependencyInjection;

namespace RepoRanger.Api.IntegrationTests.Startup;

public class DependencyInjectionTests : TestBase
{
    [Test]
    public void AllServices_ShouldBeResolvable()
    {
        var serviceDescriptors = Factory.Services.GetService<IServiceCollection>();
        Assert.That(serviceDescriptors, Is.Not.Null);

        foreach (var descriptor in serviceDescriptors)
        {
            var serviceType = descriptor.ServiceType;
            var resolvedService = GetService(serviceType);

            Assert.That(resolvedService, Is.Not.Null);
        }
    }
}