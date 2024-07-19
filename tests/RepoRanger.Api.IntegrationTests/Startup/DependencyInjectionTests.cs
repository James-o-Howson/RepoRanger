﻿using Microsoft.Extensions.Logging;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;

namespace RepoRanger.Api.IntegrationTests.Startup;

public class DependencyInjectionTests : TestBase
{
    [Test]
    public void CanResolve_CoreServices()
    {
        // var sourceParserService = GetRequiredService<ISourceParserService>();
        var logger = GetRequiredService<ILogger<DependencyInjectionTests>>();
        var dbContext = GetRequiredService<IApplicationDbContext>();
        
        Assert.Multiple(() =>
        {
            // Assert.That(sourceParserService, Is.Not.Null);
            Assert.That(logger, Is.Not.Null);
            Assert.That(dbContext, Is.Not.Null);
        });
    }
}