using Microsoft.Extensions.Logging;
using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Application.Events;
using RepoRanger.Domain.SourceParsing;

namespace RepoRanger.Api.IntegrationTests.Startup;

public class DependencyInjectionTests : TestBase
{
    [Test]
    public void CanResolve_CoreServices()
    {
        var sourceParserService = GetRequiredService<ISourceParserService>();
        var logger = GetRequiredService<ILogger<DependencyInjectionTests>>();
        var eventDispatcher = GetRequiredService<IEventDispatcher>();
        var dbContext = GetRequiredService<IApplicationDbContext>();
        
        Assert.Multiple(() =>
        {
            Assert.That(sourceParserService, Is.Not.Null);
            Assert.That(logger, Is.Not.Null);
            Assert.That(eventDispatcher, Is.Not.Null);
            Assert.That(dbContext, Is.Not.Null);
        });
    }
}