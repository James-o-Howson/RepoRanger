using RepoRanger.Domain.VersionControlSystems.ValueObjects;

namespace RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;

public record ProjectDescriptor(ProjectType Type, string Name, string Version, string Path, 
    IReadOnlyCollection<ProjectMetadataDescriptor> Metadata, 
    IReadOnlyCollection<ProjectDependencyDescriptor> ProjectDependencies);