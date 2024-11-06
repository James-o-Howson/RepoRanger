namespace RepoRanger.SharedKernel.Events.Domain;

public interface IDomainEvent
{
    DateTimeOffset OccuredOn { get; }
}