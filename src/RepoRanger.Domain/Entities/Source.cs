﻿using RepoRanger.Domain.Common.Interfaces;

namespace RepoRanger.Domain.Entities;

public sealed class Source : ICreatedAuditableEntity
{
    private readonly List<Repository> _repositories = [];
    
    private Source() { }

    public static Source Create(string name, string location) => new()
    {
        Name = name ?? throw new ArgumentNullException(nameof(name), "Cannot be null"),
        Location = location ?? throw new ArgumentNullException(nameof(location), "Cannot be null")
    };

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public IReadOnlyCollection<Repository> Repositories => _repositories;
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
    
    public IEnumerable<DependencyInstance> DependencyInstances => Repositories
        .SelectMany(r => r.DependencyInstances)
        .ToList();
    
    public void AddRepositories(IEnumerable<Repository> repositories)
    {
        ArgumentNullException.ThrowIfNull(repositories);
        _repositories.AddRange(repositories);
    }

    public void Delete()
    {
        foreach (var repository in Repositories)
        {
            repository.Delete();
        }

        _repositories.Clear();
    }
}