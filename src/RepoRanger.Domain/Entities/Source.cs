﻿using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.Entities;

public sealed class Source : Auditable, IEquatable<Source>
{
    private readonly List<Repository> _repositories = [];
    
    private Source() { }

    public static Source Create(string name, string location)
    {
        var source = new Source
        {
            Name = name ?? throw new ArgumentNullException(nameof(name), "Cannot be null"),
            Location = location ?? throw new ArgumentNullException(nameof(location), "Cannot be null")
        };
        
        return source;
    }
    
    public static Source Create(string name, string location, IEnumerable<Repository> repositories)
    {
        var source = Create(name, location);
        source.AddRepositories(repositories);
        return source;
    }

    public int Id { get; set; }
    public string Name { get; private set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public IReadOnlyCollection<Repository> Repositories => _repositories;
    public bool IsNew => Id == 0;
    
    public IEnumerable<DependencyInstance> DependencyInstances => Repositories
        .SelectMany(r => r.DependencyInstances)
        .ToList();
    
    public IEnumerable<string> Dependencies => 
        DependencyInstances.Select(di => di.DependencyName);
    
    public void Update(string location, IReadOnlyCollection<Repository> repositories)
    {
        ArgumentException.ThrowIfNullOrEmpty(location);
        ArgumentNullException.ThrowIfNull(repositories);
        
        Location = location;
        
        var removed = Repositories.Except(repositories);
        foreach (var repository in removed)
        {
            _repositories.Remove(repository);
        }
        
        var updated = repositories.Intersect(Repositories);
        foreach (var repository in updated)
        {
            var index = _repositories.IndexOf(repository);
            _repositories[index].Update(repository.DefaultBranch, repository.Projects);
        }
        
        var added = repositories.Except(Repositories);
        _repositories.AddRange(added);
    }
    
    public void AddRepositories(IEnumerable<Repository> repositories)
    {
        ArgumentNullException.ThrowIfNull(repositories);
        _repositories.AddRange(repositories);
    }
    
    public Repository? GetRepository(Repository repository)
    {
        return _repositories.FirstOrDefault(r =>
            r.SourceId == repository.SourceId && 
            r.Name == repository.Name && 
            r.RemoteUrl == repository.RemoteUrl);
    }
    
    public void Delete()
    {
        foreach (var repository in Repositories)
        {
            repository.Delete();
        }

        _repositories.Clear();
    }

    public bool Equals(Source? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Source other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}