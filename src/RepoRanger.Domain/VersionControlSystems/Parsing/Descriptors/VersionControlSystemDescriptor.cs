using RepoRanger.Domain.VersionControlSystems.AlternateKeys;

namespace RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;

public sealed record VersionControlSystemDescriptor(string Name, string Location, 
    IReadOnlyCollection<RepositoryDescriptor> Repositories) : IAlternateKeyProvider
{
    public AlternateKey GetAlternateKey => new VersionControlSystemAlternateKey(Name, Location);
}