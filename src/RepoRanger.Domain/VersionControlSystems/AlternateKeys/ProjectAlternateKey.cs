namespace RepoRanger.Domain.VersionControlSystems.AlternateKeys;

internal sealed record ProjectAlternateKey(string Name, string Path) : AlternateKey;