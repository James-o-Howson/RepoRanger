using RepoRanger.Domain.Dependencies.Contracts;
using RepoRanger.Domain.Dependencies.ValueObjects;
using RepoRanger.Domain.VersionControlSystems.Entities;

namespace RepoRanger.Domain.VersionControlSystems.AlternateKeys;

internal sealed record ProjectDependencyAlternateKey(DependencyId DependencyId, DependencyVersionId VersionId) : AlternateKey
{
    internal static ProjectDependencyAlternateKey Create(ProjectDependency entity) => 
        new(entity.DependencyId, entity.VersionId);
    
    internal static ProjectDependencyAlternateKey Create(RegistrationResult registrationResult) => 
        new(registrationResult.Dependency.Id, registrationResult.Version.Id);
};