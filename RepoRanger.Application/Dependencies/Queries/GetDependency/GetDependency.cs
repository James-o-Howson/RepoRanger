using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Common.Exceptions;
using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Domain.Entities;

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
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (dependency is null) throw new NotFoundException($"Can't find Dependency for Id {request.Id}");
        
        return new DependencyDetailVm
        {
            Id = dependency.Id,
            Name = dependency.Name,
            Version = dependency.Version,
            Repositories = dependency.Projects.Select(p => new RepositoryDetailVm
            {
                Id = p.RepositoryId,
                Name = p.Repository.Name,
                Projects = GetProjectsForRepository(p.RepositoryId, dependency.Projects)
            })
        };
    }

    private static IEnumerable<ProjectDetailVm> GetProjectsForRepository(Guid repositoryId, IEnumerable<Project> projects) => 
        projects.Where(p => p.RepositoryId == repositoryId).Select(p => new ProjectDetailVm
        {
            Id = p.Id,
            Name = p.Name,
            Version = p.Version,
        });
}