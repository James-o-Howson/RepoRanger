namespace RepoRanger.Domain.VersionControlSystems.AlternateIds;

public interface IAlternateIdProvider
{
    AlternateId GetAlternateId { get; }
}