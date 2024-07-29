namespace RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;

public record ProjectDependencyDescriptor(string Name, string Source, string? Version);