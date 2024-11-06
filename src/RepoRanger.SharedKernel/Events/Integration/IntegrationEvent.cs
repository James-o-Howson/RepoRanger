namespace RepoRanger.SharedKernel.Events.Integration;

public abstract class IntegrationEvent : IIntegrationEvent
{
    public DateTimeOffset OccuredOn { get; set; } = DateTimeOffset.UtcNow;
    
    protected IntegrationEvent() { }

    protected IntegrationEvent(DateTimeOffset occuredOn)
    {
        OccuredOn = occuredOn;
    }
    
    public abstract string AssemblyQualifiedName { get; }
}