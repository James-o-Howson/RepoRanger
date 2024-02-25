﻿using RepoRanger.Domain.Common;

namespace RepoRanger.Domain.Entities;

public class Repository : ICreatedAuditableEntity
{
    private readonly List<Project> _projects = [];
    
    private Repository() { }

    public Repository(string name, string remoteUrl, Branch defaultBranch)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(remoteUrl);
        
        Id = Guid.NewGuid();
        Name = name;
        RemoteUrl = remoteUrl;
        SetDefaultBranch(defaultBranch);
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string RemoteUrl { get; private set; }
    public Guid SourceId { get; private set; }
    public Source Source { get; set; }
    public Guid DefaultBranchId { get; private set; }
    public Branch DefaultBranch { get; private set; }
    public IReadOnlyCollection<Project> Projects => _projects;
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }

    public IEnumerable<DependencyInstance> DependencyInstances => Projects
        .SelectMany(p => p.DependencyInstances)
        .ToList();
    
    public void SetDefaultBranch(Branch branch)
    {
        ArgumentNullException.ThrowIfNull(branch);
        ArgumentNullException.ThrowIfNull(branch.Id);
        
        DefaultBranchId = branch.Id;
        DefaultBranch = branch;
    }
    
    public void AddProject(Project project)
    {
        ArgumentNullException.ThrowIfNull(project);
        _projects.Add(project);
    }
    
    public void AddProjects(IEnumerable<Project> projects)
    {
        ArgumentNullException.ThrowIfNull(projects);
        
        _projects.AddRange(projects);
    }

    internal void Delete()
    {
        foreach (var branch in Projects)
        {
            branch.Delete();
        }

        _projects.Clear();
    }
}