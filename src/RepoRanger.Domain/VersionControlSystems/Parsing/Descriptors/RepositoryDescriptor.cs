using RepoRanger.Domain.VersionControlSystems.AlternateIds;

namespace RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;

public sealed record RepositoryDescriptor(string Name, string RemoteUrl, string DefaultBranch, 
    IReadOnlyCollection<ProjectDescriptor> Projects) : IAlternateIdProvider
{
    public AlternateId GetAlternateId => new RepositoryAlternateId(Name, RemoteUrl);
}