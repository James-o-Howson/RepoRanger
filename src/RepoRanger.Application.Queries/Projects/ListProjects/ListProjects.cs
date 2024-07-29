using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Application.Contracts.Projects;

namespace RepoRanger.Application.Queries.Projects.ListProjects;

public sealed record ListProjectsQuery : IRequest<ProjectsVm>; 

internal sealed class ListProjectsQueryHandler : IRequestHandler<ListProjectsQuery, ProjectsVm>
{
    private readonly IApplicationDbContext _context;

    public ListProjectsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectsVm> Handle(ListProjectsQuery request, CancellationToken cancellationToken)
    {
        return new ProjectsVm
        {
            Projects = await _context.Projects
                .AsNoTracking()
                .Include(p => p.ProjectDependencies)
                .Include(p => p.Repository)
                .Select(p => new ProjectVm
                {
                    Id = p.Id,
                    Type = p.Type,
                    Name = p.Name,
                    Version = p.Version,
                    DependencyCount = p.ProjectDependencies.Count,
                    RepositoryId = p.RepositoryId,
                    RepositoryName = p.Repository.Name,

                })
                .ToListAsync(cancellationToken)
        };
    }
}