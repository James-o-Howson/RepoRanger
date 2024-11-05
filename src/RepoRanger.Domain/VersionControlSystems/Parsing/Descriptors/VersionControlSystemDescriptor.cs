using RepoRanger.Domain.VersionControlSystems.ValueObjects.AlternateIds;

namespace RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;

public sealed record VersionControlSystemDescriptor(string Name, string Location, 
    IReadOnlyCollection<RepositoryDescriptor> Repositories) : IAlternateIdProvider
{
    public AlternateId GetAlternateId => new VersionControlSystemAlternateId(Name, Location);
}