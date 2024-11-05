namespace RepoRanger.Domain.VersionControlSystems.ValueObjects.AlternateIds;

internal sealed record RepositoryAlternateId(string Name, string RemoteUrl) : AlternateId;