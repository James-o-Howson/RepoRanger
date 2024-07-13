using Microsoft.Extensions.DependencyInjection;
using RepoRanger.Domain.Common.Events;

namespace RepoRanger.Application.Events;

public interface ITransientEventDispatcher
{
    Task DispatchAsync(IEvent @event, CancellationToken cancellationToken);
}

internal sealed class TransientEventDispatcher : ITransientEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public TransientEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(IEvent @event, CancellationToken cancellationToken) => 
        await DispatchEventAsync(@event, cancellationToken);

    private async Task DispatchEventAsync(IEvent @event, CancellationToken cancellationToken)
    {
        var eventHandlerType = typeof(ITransientEventHandler<>);
        var handlerType = eventHandlerType.MakeGenericType(@event.GetType());
        var handlers = _serviceProvider.GetServices(handlerType);
        
        foreach (var handler in handlers)
        {
            var handleMethod = handlerType.GetMethod(ITransientEventHandler<IEvent>.HandleMethodName);
            if (handleMethod is null || handler is null) return;
            
            await (Task) handleMethod.Invoke(handler, [@event, cancellationToken])!;
        }
    }
}