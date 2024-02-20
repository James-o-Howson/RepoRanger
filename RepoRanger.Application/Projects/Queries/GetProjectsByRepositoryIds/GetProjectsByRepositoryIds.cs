using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Application.Projects.ViewModels;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.Projects.Queries.GetProjectsByRepositoryIds;

public sealed record GetProjectsByRepositoryIdsQuery : IRequest<ProjectsVm>
{
    public List<Guid> RepositoryIds { get; init; }
    public bool DefaultBranchOnly { get; init; } = true;
}

internal sealed class GetProjectsByRepositoryIdsQueryHandler : IRequestHandler<GetProjectsByRepositoryIdsQuery, ProjectsVm>
{
    private readonly IApplicationDbContext _context;

    public GetProjectsByRepositoryIdsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectsVm> Handle(GetProjectsByRepositoryIdsQuery request, CancellationToken cancellationToken)
    {
        var repositories = await _context.Repositories
            .AsNoTracking()
            .Include(r => r.DefaultBranch)
                .ThenInclude(b => b.Projects)
                .ThenInclude(p => p.Dependencies)
            .Include(r => r.Branches)
                .ThenInclude(b => b.Projects)
                .ThenInclude(p => p.Dependencies)
            .Where(r => request.RepositoryIds.Contains(r.Id))
            .ToListAsync(cancellationToken);

        var projectVms = new List<ProjectVm>();
        foreach (var repository in repositories)
        {
            projectVms.AddRange(request.DefaultBranchOnly
                ? repository.DefaultBranch.Projects.Select(p => ToProjectVm(p, repository))
                : repository.Branches.SelectMany(b => b.Projects).Select(p => ToProjectVm(p, repository)));
        }

        return new ProjectsVm
        {
            Projects = projectVms.ToList()
        };
    }

    private static ProjectVm ToProjectVm(Project p, Repository repository)
    {
        return new ProjectVm
        {
            Id = p.Id,
            Name = p.Name,
            Version = p.Version,
            DependencyCount = p.Dependencies.Count,
            RepositoryId = repository.Id,
            RepositoryName = repository.Name
        };
    }
}