using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RepoRanger.Abstractions.Events.Integration;
using RepoRanger.Domain.OutboxMessages;
using RepoRanger.Domain.OutboxMessages.ValueObjects;
using RepoRanger.Domain.OutboxMessages.ValueObjects.Enums;
using SharedKernel.Abstractions;

namespace RepoRanger.Infrastructure.OutboxMessages;

internal sealed class OutboxMessageDispatcher : IOutboxMessageDispatcher
{
    private readonly ILogger<OutboxMessageDispatcher> _logger;
    private readonly IMediator _mediator;
    private readonly TimeProvider _timeProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutboxMessageRepository _outboxMessageRepository;
    private readonly OutboxProcessingOptions _outboxProcessingOptions;

    public OutboxMessageDispatcher(ILogger<OutboxMessageDispatcher> logger,
        IMediator mediator,
        TimeProvider timeProvider, 
        IUnitOfWork unitOfWork, 
        IOptions<OutboxProcessingOptions> outboxProcessingOptions, 
        IOutboxMessageRepository outboxMessageRepository)
    {
        _logger = logger;
        _mediator = mediator;
        _timeProvider = timeProvider;
        _unitOfWork = unitOfWork;
        _outboxProcessingOptions = outboxProcessingOptions.Value;
        _outboxMessageRepository = outboxMessageRepository;
    }

    public async Task ProcessAsync(CancellationToken cancellationToken = default)
    {
        var outboxMessages =
            await _outboxMessageRepository.GetPendingMessagesAsync(_outboxProcessingOptions.BatchSize, cancellationToken);
        
        foreach (var outboxMessage in outboxMessages)
        {
            await ProcessAsync(outboxMessage, cancellationToken);
        }
    }

    public async Task ProcessAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken = default)
    {
        try
        {
            outboxMessage.StartProcessing(_timeProvider.GetUtcNow());
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Processing Outbox Message: {OutboxMessageId}", outboxMessage.Id);
            
            await _mediator.Publish(outboxMessage.Event.ToNotification(), cancellationToken);
                
            outboxMessage.Complete(_timeProvider.GetUtcNow());
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Finished Processing Outbox Message: {OutboxMessageId}", outboxMessage.Id);
        }
        catch (Exception exception)
        {
            var error = Error.CreateInstance(exception, ErrorSeverity.Permanent);
            outboxMessage.RecordFailure(error, _timeProvider.GetUtcNow());
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogError(exception, "Failed to Process Persisted Event: {PersistedJobName}", outboxMessage.Id);
        }
    }
}