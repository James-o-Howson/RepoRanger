namespace RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;

public record ProjectDependencyDescriptor(string Name, string Source, string? Version);