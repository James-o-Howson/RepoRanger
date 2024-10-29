using RepoRanger.Domain.Dependencies.Contracts;
using RepoRanger.Domain.Dependencies.ValueObjects;
using RepoRanger.Domain.VersionControlSystems.Entities;

namespace RepoRanger.Domain.VersionControlSystems.AlternateIds;

internal sealed record ProjectDependencyAlternateId(DependencyId DependencyId, DependencyVersionId VersionId) : AlternateId
{
    internal static ProjectDependencyAlternateId Create(ProjectDependency entity) => 
        new(entity.DependencyId, entity.VersionId);
    
    internal static ProjectDependencyAlternateId Create(RegistrationResult registrationResult) => 
        new(registrationResult.Dependency.Id, registrationResult.Version.Id);
};