using MediatR;

namespace RepoRanger.Domain.Events;

public abstract class IntegrationEvent : INotification
{
    public DateTimeOffset OccuredOn { get; set; } = DateTimeOffset.UtcNow;
    
    protected IntegrationEvent() { }

    protected IntegrationEvent(DateTimeOffset occuredOn)
    {
        OccuredOn = occuredOn;
    }
    
    public abstract string AssemblyQualifiedName { get; }
}