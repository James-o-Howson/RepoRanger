using RepoRanger.Domain.VersionControlSystems.AlternateIds;
using RepoRanger.Domain.VersionControlSystems.ValueObjects;

namespace RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;

public record ProjectDescriptor(ProjectType Type, string Name, string Version, string Path, 
    IReadOnlyCollection<ProjectMetadataDescriptor> Metadata, 
    IReadOnlyCollection<ProjectDependencyDescriptor> ProjectDependencies) : IAlternateIdProvider
{
    public AlternateId GetAlternateId => new ProjectAlternateId(Name, Path);
}