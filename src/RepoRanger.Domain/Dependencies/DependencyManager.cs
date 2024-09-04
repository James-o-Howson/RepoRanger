using RepoRanger.Domain.Common.Exceptions;
using RepoRanger.Domain.Dependencies.Contracts;
using RepoRanger.Domain.Dependencies.Entities;
using RepoRanger.Domain.Dependencies.ValueObjects;

namespace RepoRanger.Domain.Dependencies;

public interface IDependencyManager : IDisposable
{
    void Manage(List<Dependency> dependencies, List<DependencyVersion> versions, List<DependencySource> sources);
    RegistrationResult Register(string dependencyName, DependencySourceName sourceName, string? versionValue);
}

internal sealed class DependencyManager : IDependencyManager
{
    private List<Dependency> _dependencies = [];
    private List<DependencyVersion> _versions = [];
    private List<DependencySource> _sources = [];
    private bool _initialised;

    public void Manage(List<Dependency> dependencies, List<DependencyVersion> versions, List<DependencySource> sources)
    {
        if (_initialised) throw new InvalidOperationException($"{nameof(DependencyManager)} is already initialised.");
        
        _initialised = true;
        _dependencies = dependencies;
        _versions = versions;
        _sources = sources;
    }
    
    public RegistrationResult Register(string dependencyName, DependencySourceName sourceName, string? versionValue)
    {
        if (!_initialised) throw new InvalidOperationException($"{nameof(DependencyManager)} is not initialised.");
        
        var existing = _dependencies.FirstOrDefault(d => d.Name == dependencyName);
        return existing is null ?
            RegisterNewDependency(dependencyName, sourceName, versionValue) : 
            RegisterExisting(existing, sourceName, versionValue);
    }

    private RegistrationResult RegisterNewDependency(string dependencyName, DependencySourceName sourceName, string? versionValue)
    {
        var dependency = Dependency.Create(dependencyName);
        var source = GetOrCreateSource(sourceName);
        var version = GetOrCreateVersion(dependency, source, versionValue);
        
        _dependencies.Add(dependency);
        dependency.TryAddVersion(version);
        return new RegistrationResult(dependency, version, source);
    }

    private RegistrationResult RegisterExisting(Dependency dependency, DependencySourceName sourceName, string? versionValue)
    {
        var source = GetOrCreateSource(sourceName);
        var version = GetOrCreateVersion(dependency, source, versionValue);
        
        return new RegistrationResult(dependency, version, source);;
    }

    private DependencyVersion GetOrCreateVersion(Dependency dependency, DependencySource source, string? versionValue)
    {
        var version = _versions.FirstOrDefault(v => 
            v.DependencyId == dependency.Id && 
            v.Value == versionValue);
        
        if (version is null)
        {
            version = DependencyVersion.Create(dependency, source, versionValue);
            _versions.Add(version);
        }
        else
        {
            version.TryAddSource(source);
        }

        dependency.TryAddVersion(version);
        return version;
    }

    private DependencySource GetOrCreateSource(DependencySourceName sourceName)
    {
        var source = _sources.FirstOrDefault(s => s.Name == sourceName);
        if (source != null) return source;
        
        source = DependencySource.Create(sourceName);
        _sources.Add(source);

        return source;
    }

    public void Dispose()
    {
        _dependencies.Clear();
        _versions.Clear();
        _sources.Clear();
        _initialised = false;
    }
}