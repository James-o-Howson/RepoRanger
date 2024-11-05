using RepoRanger.Domain.Dependencies.Entities;
using RepoRanger.Domain.VersionControlSystems.ValueObjects.AlternateIds;

namespace RepoRanger.Domain.Dependencies.Contracts;

public sealed record RegistrationResult(Dependency Dependency, DependencyVersion Version, DependencySource Source) : IAlternateIdProvider
{
    public AlternateId GetAlternateId => new ProjectDependencyAlternateId(Dependency.Id, Version.Id);
}