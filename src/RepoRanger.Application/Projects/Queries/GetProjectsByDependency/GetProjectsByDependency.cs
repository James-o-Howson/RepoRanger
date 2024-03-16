using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Application.Projects.ViewModels;

namespace RepoRanger.Application.Projects.Queries.GetProjectsByDependency;

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
            .Include(p => p.DependencyInstances)
            .Where(p => p.DependencyInstances.Any(di => di.DependencyName == request.DependencyName))
            .ToListAsync(cancellationToken);

        if (request.Version != null)
        {
            projects = projects.Where(p => p.HasDependency(request.DependencyName, request.Version)).ToList();
        }

        var viewModels = projects.Select(p => new ProjectVm
        {
            Id = p.Id,
            Name = p.Name,
            Type = p.Type,
            Version = p.Version,
            DependencyCount = p.DependencyInstances.Count,
            RepositoryId = p.RepositoryId,
            RepositoryName = p.Repository.Name
        }).ToList();

        return new ProjectsVm
        {
            Projects = viewModels
        };
    }
}