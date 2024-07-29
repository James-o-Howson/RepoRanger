namespace RepoRanger.Domain.VersionControlSystems.Parsing.Contracts;

public sealed record RepositoryDescriptor(string Name, string RemoteUrl, string DefaultBranch, 
    IReadOnlyCollection<ProjectDescriptor> Projects);