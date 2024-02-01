﻿using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Application.ViewModels;
using RepoRanger.Domain.Entities;
using RepoRanger.Domain.ValueObjects;

namespace RepoRanger.Application.Services;

public interface IRepositoryService
{
    Task SaveAsync(SourceViewModel sourceViewModel, CancellationToken cancellationToken);
}

public sealed class RepositoryService : IRepositoryService
{
    private readonly IApplicationDbContext _applicationDbContext;

    public RepositoryService(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task SaveAsync(SourceViewModel sourceViewModel, CancellationToken cancellationToken)
    {
        var uniqueDependencies = await GetUniqueRepositoryDependencies(sourceViewModel, cancellationToken);
        
        var source = GetSource(sourceViewModel, uniqueDependencies);
        await _applicationDbContext.Sources.AddAsync(source, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<List<Dependency>> GetUniqueRepositoryDependencies(SourceViewModel sourceViewModel, CancellationToken cancellationToken)
    {
        var allExistingDependencies = await _applicationDbContext.Dependencies.ToListAsync(cancellationToken);
        var dependencyViewModels = sourceViewModel.Repositories
            .SelectMany(r => r.Branches)
            .SelectMany(b => b.Projects)
            .SelectMany(p => p.Dependencies)
            .GroupBy(d => new { d.Name, d.Version })
            .Select(g => g.First())
            .ToList();

        var existingDependencies = new List<Dependency>();
        foreach (var existingDependency in allExistingDependencies)
        {
            var matchingViewModel = dependencyViewModels
                .FirstOrDefault(vm => vm.Name == existingDependency.Name && vm.Version == existingDependency.Version);

            if (matchingViewModel == null) continue;
            
            // Use the existing Dependency
            existingDependencies.Add(existingDependency);
            dependencyViewModels.Remove(matchingViewModel);
        }
        
        // Transform remaining new DependencyViewModels into Dependency objects
        var newDependencies = dependencyViewModels
            .Select(v => new Dependency(v.Name, v.Version))
            .ToList();
        
        await _applicationDbContext.Dependencies.AddRangeAsync(newDependencies, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        existingDependencies.AddRange(newDependencies);
        return existingDependencies;
    }

    private static Source GetSource(SourceViewModel sourceViewModel, IEnumerable<Dependency> dependencies)
    {
        var source = new Source(sourceViewModel.Name);
        var repositories = GetRepositories(sourceViewModel.Repositories, dependencies);
        source.AddRepositories(repositories);

        return source;
    }

    private static IEnumerable<Repository> GetRepositories(IEnumerable<RepositoryViewModel> repositoryViewModels,
        IEnumerable<Dependency> dependencies)
    {
        return repositoryViewModels.Select(r =>
        {
            var repository = new Repository(r.Name, r.Url, r.RemoteUrl);
            var branches = GetBranches(r.Branches, dependencies);
            repository.AddBranches(branches);

            return repository;
        });
    }

    private static IEnumerable<Branch> GetBranches(IEnumerable<BranchViewModel> branchViewModels,
        IEnumerable<Dependency> dependencies)
    {
        var branches = branchViewModels.Select(b =>
        {
            var branch = new Branch(b.Name);
            var projects = GetProjects(b.Projects, dependencies);
            branch.AddProjects(projects);
                
            return branch;
        });

        return branches;
    }

    private static IEnumerable<Project> GetProjects(IEnumerable<ProjectViewModel> projectViewModels,
        IEnumerable<Dependency> dependencies)
    {
        var projects = projectViewModels.Select(p =>
        {
            var projectDependencies = dependencies.Where(d =>
                p.Dependencies.Any(vm => vm.Name == d.Name && vm.Version == d.Version));
            
            var project = new Project(p.Name, ProjectType.From(p.TypeName, p.TypeVersion));
            project.AddDependencies(projectDependencies);
            
            return project;
        });

        return projects;
    }
    
    private static IEnumerable<Dependency> GetDependencies(SourceViewModel sourceViewModel) =>
        sourceViewModel.Repositories
            .SelectMany(r => r.Branches)
            .SelectMany(b => b.Projects)
            .SelectMany(p => p.Dependencies)
            .GroupBy(d => new { d.Name, d.Version })
            .Select(g => g.First())
            .Select(d => new Dependency(d.Name, d.Version));
}