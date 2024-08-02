using RepoRanger.Domain.Common.Exceptions;
using RepoRanger.Domain.Dependencies.Contracts;
using RepoRanger.Domain.Dependencies.Entities;

namespace RepoRanger.Domain.Dependencies;

public interface IDependencyManager : IDisposable
{
    void Manage(List<Dependency> dependencies, List<DependencyVersion> versions, List<DependencySource> sources);
    RegistrationResult Register(string dependencyName, string sourceName, string? versionValue);
}

internal sealed class DependencyManager : IDependencyManager
{
    private List<Dependency> _dependencies = [];
    private List<DependencyVersion> _versions = [];
    private List<DependencySource> _sources = [];
    private bool _initialised;

    public void Manage(List<Dependency> dependencies, List<DependencyVersion> versions, List<DependencySource> sources)
    {
        if (_initialised) throw new InvalidOperationException("DependencyManager is already initialised.");
        DomainException.ThrowIfNull(dependencies);
        DomainException.ThrowIfNull(versions);
        DomainException.ThrowIfNull(sources);
        
        _initialised = true;
        _dependencies = dependencies;
        _versions = versions;
        _sources = sources;
    }
    
    public RegistrationResult Register(string dependencyName, string sourceName, string? versionValue)
    {
        EnsureInitialized();
        DomainException.ThrowIfNullOrEmpty(dependencyName);
        DomainException.ThrowIfNullOrEmpty(sourceName);
        
        var existing = _dependencies.FirstOrDefault(d => d.Name == dependencyName);
        if (existing is null)
        {
            return RegisterNewDependency(dependencyName, sourceName, versionValue);
        }

        return HandleExisting(existing, sourceName, versionValue);
    }

    private RegistrationResult RegisterNewDependency(string dependencyName, string sourceName, string? versionValue)
    {
        var dependency = Dependency.Create(dependencyName);
        var source = DependencySource.Create(sourceName);
        var version = DependencyVersion.Create(dependency, source, versionValue);
        
        _dependencies.Add(dependency);
        return new RegistrationResult(dependency, version, source);
    }

    private RegistrationResult HandleExisting(Dependency dependency, string sourceName, string? versionValue)
    {
        var source = GetOrCreateSource(sourceName);
        var version = TryGetVersion(dependency.Id, versionValue);
        
        if (version is null)
        {

            version = DependencyVersion.Create(dependency, source, versionValue);
        }
        else
        {
            version.AddSource(source);
        }
        
        dependency.AddVersion(version);
        return new RegistrationResult(dependency, version, source);;
    }

    private DependencyVersion? TryGetVersion(Guid dependencyId, string? versionValue) => 
        _versions.FirstOrDefault(v => v.DependencyId == dependencyId && v.Value == versionValue);

    private DependencySource GetOrCreateSource(string sourceName) =>
        _sources.FirstOrDefault(s => s.Name == sourceName) ?? 
        DependencySource.Create(sourceName);

    private void EnsureInitialized()
    {
        if (!_initialised) throw new InvalidOperationException("DependencyManager is not initialised.");
    }

    public void Dispose()
    {
        _dependencies.Clear();
        _versions.Clear();
        _sources.Clear();
        _initialised = false;
    }
}