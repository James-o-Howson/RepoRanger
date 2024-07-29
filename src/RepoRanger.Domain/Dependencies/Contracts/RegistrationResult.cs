using RepoRanger.Domain.Dependencies.Entities;

namespace RepoRanger.Domain.Dependencies.Contracts;

public sealed record RegistrationResult(Dependency Dependency, DependencyVersion Version, DependencySource Source);