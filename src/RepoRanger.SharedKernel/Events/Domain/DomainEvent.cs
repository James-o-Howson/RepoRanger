namespace RepoRanger.SharedKernel.Events.Domain;

public abstract class DomainEvent : IDomainEvent
{
    public DateTimeOffset OccuredOn { get; }

    protected DomainEvent(DateTimeOffset occuredOn)
    {
        OccuredOn = occuredOn;
    }
}