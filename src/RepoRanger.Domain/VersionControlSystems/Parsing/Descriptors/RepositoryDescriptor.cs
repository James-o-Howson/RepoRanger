using RepoRanger.Domain.VersionControlSystems.AlternateKeys;

namespace RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;

public sealed record RepositoryDescriptor(string Name, string RemoteUrl, string DefaultBranch, 
    IReadOnlyCollection<ProjectDescriptor> Projects) : IAlternateKeyProvider
{
    public AlternateKey GetAlternateKey { get; }
}