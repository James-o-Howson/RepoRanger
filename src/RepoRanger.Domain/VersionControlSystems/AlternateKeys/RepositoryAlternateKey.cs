namespace RepoRanger.Domain.VersionControlSystems.AlternateKeys;

internal sealed record RepositoryAlternateKey(string Name, string RemoteUrl) : AlternateKey;