using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Abstractions.Interfaces.Data;
using RepoRanger.Contracts.Projects;

namespace RepoRanger.Queries.Projects.ListProjects;

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
                    Id = p.Id.Value,
                    Type = p.Type,
                    Name = p.Name,
                    Version = p.Version,
                    DependencyCount = p.ProjectDependencies.Count,
                    RepositoryId = p.RepositoryId.Value,
                    RepositoryName = p.Repository.Name,

                })
                .ToListAsync(cancellationToken)
        };
    }
}