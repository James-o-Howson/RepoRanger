namespace RepoRanger.Domain.VersionControlSystems.ValueObjects.AlternateIds;

internal sealed record ProjectAlternateId(string Name, string Path) : AlternateId;