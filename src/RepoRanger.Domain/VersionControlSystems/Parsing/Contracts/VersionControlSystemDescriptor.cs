namespace RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;

public sealed record VersionControlSystemDescriptor(string Name, string Location, 
    IReadOnlyCollection<RepositoryDescriptor> Repositories);