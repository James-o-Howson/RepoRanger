using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RepoRanger.Abstractions.Interfaces.Data;
using RepoRanger.Domain.OutboxMessages;
using RepoRanger.Domain.OutboxMessages.ValueObjects;
using RepoRanger.Domain.OutboxMessages.ValueObjects.Enums;

namespace RepoRanger.Infrastructure.OutboxMessages;

internal sealed class OutboxMessageProcessor : IOutboxMessageProcessor
{
    private readonly ILogger<OutboxMessageProcessor> _logger;
    private readonly IMediator _mediator;
    private readonly TimeProvider _timeProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutboxMessageRepository _outboxMessageRepository;
    private readonly OutboxProcessingOptions _outboxProcessingOptions;

    public OutboxMessageProcessor(ILogger<OutboxMessageProcessor> logger,
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
            
            await _mediator.Publish(outboxMessage.Event, cancellationToken);
                
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