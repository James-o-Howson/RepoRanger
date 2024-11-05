namespace RepoRanger.Domain.VersionControlSystems.ValueObjects.AlternateIds;

internal sealed record VersionControlSystemAlternateId(string Name, string Location) : AlternateId;