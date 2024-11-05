using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using RepoRanger.Abstractions.Interfaces;
using RepoRanger.Contracts.Vulnerabilities.External.Response;
using RepoRanger.Contracts.Vulnerabilities.IntegrationEvents;
using RepoRanger.Domain.Dependencies.ValueObjects;
using RepoRanger.Domain.OutboxMessages;

namespace RepoRanger.Api.IntegrationTests.Infrastructure;

public class OutboxMessageProcessorTests : TestBase
{
    private IOutboxMessageProcessor _processor;
    private TimeProvider _timeProvider;
    private IExternalVulnerabilityService _externalVulnerabilityService;

    [SetUp]
    public void SetUp()
    {
        _processor = GetRequiredService<IOutboxMessageProcessor>();
        _timeProvider = GetRequiredService<TimeProvider>();
        _externalVulnerabilityService = Substitute.For<IExternalVulnerabilityService>();
        _externalVulnerabilityService.QueryAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new ExternalVulnerability()));
    }
    
    [Test]
    public async Task DispatchAsync_ValidOutboxMessage_DispatchesSuccessfully()
    {
        var @event = new VulnerabilityDiscoveredIntegrationEvent
        {
            VulnerabilityId = new VulnerabilityId(Guid.Parse("22C33D7F-154E-45FC-B111-FA215C84AB95"))
        };
        
        var outboxMessage = OutboxMessage.Create(@event, _timeProvider.GetUtcNow());
    
        await _processor.ProcessAsync(outboxMessage);
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IExternalVulnerabilityService>(_ => _externalVulnerabilityService);
    }
}