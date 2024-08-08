using RepoRanger.Domain.VersionControlSystems.AlternateKeys;
using RepoRanger.Domain.VersionControlSystems.ValueObjects;

namespace RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;

public record ProjectDescriptor(ProjectType Type, string Name, string Version, string Path, 
    IReadOnlyCollection<ProjectMetadataDescriptor> Metadata, 
    IReadOnlyCollection<ProjectDependencyDescriptor> ProjectDependencies) : IAlternateKeyProvider
{
    public AlternateKey GetAlternateKey => new ProjectAlternateKey(Name, Path);
}