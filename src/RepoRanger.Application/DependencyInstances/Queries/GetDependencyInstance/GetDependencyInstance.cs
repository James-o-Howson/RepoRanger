using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Common.Exceptions;
using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.DependencyInstances.Queries.GetDependencyInstance;

public sealed record GetDependencyInstanceQuery : IRequest<DependencyInstanceDetailVm>
{
    public Guid Id { get; init; }
}

internal sealed class GetDependencyInstanceQueryHandler : IRequestHandler<GetDependencyInstanceQuery, DependencyInstanceDetailVm>
{
    private readonly IApplicationDbContext _context;

    public GetDependencyInstanceQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DependencyInstanceDetailVm> Handle(GetDependencyInstanceQuery request, CancellationToken cancellationToken)
    {
        var dependencyInstance = await _context.DependencyInstances
            .Include(d => d.Project)
            .ThenInclude(b => b.Repository)
            .AsSplitQuery()
            .Select(d => new DependencyInstanceDetailVm
            {
                Id = d.Id,
                Source = d.Source,
                Name = d.DependencyName,
                Version = d.Version
            })
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);
        
        if (dependencyInstance is null) throw new NotFoundException($"Can't find Dependency for Id {request.Id}");

        return dependencyInstance;
    }

    private static IEnumerable<ProjectDetailVm> GetProjectsForRepository(Guid repositoryId, IEnumerable<Project> projects) => 
        projects.Where(p => p.RepositoryId == repositoryId).Select(p => new ProjectDetailVm
        {
            Id = p.Id,
            Name = p.Name,
            Version = p.Version,
        });
}