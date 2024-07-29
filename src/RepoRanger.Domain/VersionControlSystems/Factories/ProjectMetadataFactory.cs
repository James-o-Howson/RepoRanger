using RepoRanger.Domain.VersionControlSystems.Entities;
using RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;

namespace RepoRanger.Domain.VersionControlSystems.Factories;

internal interface IProjectMetadataFactory
{
    IEnumerable<ProjectMetadata> Create(IEnumerable<ProjectMetadataDescriptor> descriptors);
}

internal sealed class ProjectMetadataFactory : IProjectMetadataFactory
{
    public IEnumerable<ProjectMetadata> Create(IEnumerable<ProjectMetadataDescriptor> descriptors) =>
        descriptors.Select(d => ProjectMetadata.Create(d.Key, d.Value));
    
    private ProjectMetadata Create(ProjectMetadataDescriptor descriptor) =>
        ProjectMetadata.Create(descriptor.Key, descriptor.Value);
}