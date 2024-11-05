namespace RepoRanger.Domain.VersionControlSystems.ValueObjects.AlternateIds;

public interface IAlternateIdProvider
{
    AlternateId GetAlternateId { get; }
}