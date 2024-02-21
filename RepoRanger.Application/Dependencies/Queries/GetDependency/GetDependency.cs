using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Common.Exceptions;
using RepoRanger.Application.Common.Interfaces.Persistence;

namespace RepoRanger.Application.Dependencies.Queries.GetDependency;

public sealed record GetDependencyQuery : IRequest<DependencyDetailVm>
{
    public Guid Id { get; init; }
}

public class DependencyDetailVm
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Version { get; init; }
    public IEnumerable<RepositoryDetailVm> Repositories { get; set; }
}

internal sealed class GetDependencyQueryHandler : IRequestHandler<GetDependencyQuery, DependencyDetailVm>
{
    private readonly IApplicationDbContext _context;

    public GetDependencyQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DependencyDetailVm> Handle(GetDependencyQuery request, CancellationToken cancellationToken)
    {
        var dependency = await _context.Dependencies
            .Include(d => d.Projects)
            .ThenInclude(b => b.Repository)
            .Select(d => new DependencyDetailVm
            {
                Id = d.Id,
                Name = d.Name,
                Version = d.Version,
                Repositories = d.Projects.Select(p => new RepositoryDetailVm
                {
                    Id = p.RepositoryId,
                    Name = p.Repository.Name,
                    TotalProjects = p.Repository.Projects.Count,
                    Projects = d.Projects.Select(p => new ProjectDetailVm
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Version = p.Version,
                        TotalDependencies = p.Dependencies.Count
                    })
                })
            })
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (dependency is null) throw new NotFoundException($"Can't find Dependency for Id {request.Id}");
        
        return dependency;
    }
}

public class ProjectDetailVm
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Version { get; init; }
    public int TotalDependencies { get; init; }
}

public class RepositoryDetailVm
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public IEnumerable<ProjectDetailVm> Projects { get; init; }
    public int TotalProjects { get; init; }
}