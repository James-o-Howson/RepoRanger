namespace RepoRanger.Domain.VersionControlSystems.Parsing.Descriptors;

public sealed record VersionControlSystemDescriptor(string Name, string Location, 
    IReadOnlyCollection<RepositoryDescriptor> Repositories);