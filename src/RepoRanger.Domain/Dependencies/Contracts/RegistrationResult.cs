using RepoRanger.Domain.Dependencies.Entities;
using RepoRanger.Domain.VersionControlSystems.AlternateKeys;

namespace RepoRanger.Domain.Dependencies.Contracts;

public sealed record RegistrationResult(Dependency Dependency, DependencyVersion Version, DependencySource Source) : IAlternateKeyProvider
{
    public AlternateKey GetAlternateKey { get; }
}