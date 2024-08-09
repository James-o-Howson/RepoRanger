using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Application.Contracts.Projects;

namespace RepoRanger.Application.Queries.Projects.GetProjectsByDependency;

public sealed record GetProjectsByDependencyQuery : IRequest<ProjectsVm>
{
    public string DependencyName { get; init; } = string.Empty;
    public string? Version { get; init; } = null;
}

internal sealed class GetProjectsByDependencyQueryHandler : IRequestHandler<GetProjectsByDependencyQuery, ProjectsVm>
{
    private readonly IApplicationDbContext _context;

    public GetProjectsByDependencyQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectsVm> Handle(GetProjectsByDependencyQuery request, CancellationToken cancellationToken)
    {
        var projects = await _context.Projects
            .AsNoTracking()
            .Include(p => p.Repository)
            .Include(p => p.ProjectDependencies)
            .ThenInclude(p => p.Dependency)
            .Where(p => p.ProjectDependencies.Any(di => di.Dependency.Name == request.DependencyName))
            .ToListAsync(cancellationToken);

        if (request.Version != null)
        {
            projects = projects.Where(p => p.HasSpecificDependency(request.DependencyName, request.Version)).ToList();
        }

        var viewModels = projects.Select(p => new ProjectVm
        {
            Id = p.Id.Value,
            Name = p.Name,
            Type = p.Type,
            Version = p.Version,
            DependencyCount = p.ProjectDependencies.Count,
            RepositoryId = p.RepositoryId.Value,
            RepositoryName = p.Repository.Name
        }).ToList();

        return new ProjectsVm
        {
            Projects = viewModels
        };
    }
}