namespace RepoRanger.Domain.VersionControlSystems.AlternateIds;

internal sealed record RepositoryAlternateId(string Name, string RemoteUrl) : AlternateId;