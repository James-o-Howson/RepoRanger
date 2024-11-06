namespace RepoRanger.SharedKernel.Events.Integration;

public interface IIntegrationEvent
{
    DateTimeOffset OccuredOn { get; set; }
    string AssemblyQualifiedName { get; }
}