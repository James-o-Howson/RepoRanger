﻿using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.Entities;

public class Dependency : BaseCreatedAuditableEntity<Guid>
{
    private readonly List<Project> _projects = [];
    
    private Dependency() { }

    public Dependency(string name, string version)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        
        Id = Guid.NewGuid();
        Name = name;
        Version = version;
    }
    
    public string Name { get; private set; }
    public string Version { get; private set; }
    public IReadOnlyCollection<Project> Projects => _projects;

    public void Delete()
    {
        _projects.Clear();
    }
}