namespace RepoRanger.Domain.VersionControlSystems.AlternateIds;

internal sealed record ProjectAlternateId(string Name, string Path) : AlternateId;