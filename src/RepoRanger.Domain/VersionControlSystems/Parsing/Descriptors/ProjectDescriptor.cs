using RepoRanger.Domain.VersionControlSystems.ValueObjects;
using RepoRanger.Domain.VersionControlSystems.ValueObjects.AlternateIds;

namespace RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;

public record ProjectDescriptor(ProjectType Type, string Name, string Version, string Path, 
    IReadOnlyCollection<ProjectMetadataDescriptor> Metadata, 
    IReadOnlyCollection<ProjectDependencyDescriptor> ProjectDependencies) : IAlternateIdProvider
{
    public AlternateId GetAlternateId => new ProjectAlternateId(Name, Path);
}