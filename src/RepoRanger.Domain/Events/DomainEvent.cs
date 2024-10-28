using MediatR;

namespace RepoRanger.Domain.Events;

public abstract class DomainEvent : INotification
{
    public DateTimeOffset OccuredOn { get; }

    protected DomainEvent(DateTimeOffset occuredOn)
    {
        OccuredOn = occuredOn;
    }
}