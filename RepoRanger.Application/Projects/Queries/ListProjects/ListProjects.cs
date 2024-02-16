using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Application.Projects.ViewModels;

namespace RepoRanger.Application.Projects.Queries.ListProjects;

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
                .Include(p => p.Dependencies)
                .Select(p => new ProjectVm
                {
                    Id = p.Id,
                    Name = p.Name,
                    Version = p.Version,
                    DependencyCount = p.Dependencies.Count
                })
                .ToListAsync(cancellationToken)
        };
    }
}